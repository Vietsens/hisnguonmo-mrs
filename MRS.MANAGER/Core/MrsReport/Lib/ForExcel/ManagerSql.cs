using Inventec.Core;
using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Base;
using MOS.EFMODEL;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisCashierRoom;
using MRS.MANAGER.Config;
using System.Data;
using MOS.MANAGER.HisSereServ;
using System.Reflection;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using MOS.DAO.Sql;

namespace MRS.MANAGER.Core.MrsReport.Lib
{
    public partial class ManagerSql : BusinessBase
    {

        public List<System.Data.DataTable> GetSum<Tfilter>(Tfilter filter, string query)
        {
            List<System.Data.DataTable> result = new List<DataTable>();
            try
            {
                if (!String.IsNullOrWhiteSpace(query))
                {
                    string jsonFilter = Newtonsoft.Json.JsonConvert.SerializeObject(filter, Newtonsoft.Json.Formatting.None);
                    Dictionary<string, object> dicFilter = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonFilter);
                    result = this.GetSum(dicFilter, query);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        internal List<System.Data.DataTable> GetSum(Dictionary<string, object> filter, string query)
        {
            List<System.Data.DataTable> result = new List<DataTable>();
            try
            {
                if (!String.IsNullOrWhiteSpace(query))
                {
                    List<string> blackHoles = GetBlackHole(query);
                    if (blackHoles != null)
                    {
                        blackHoles = blackHoles.Distinct().OrderByDescending(o => o.Length).ToList();
                        foreach (var item in blackHoles)
                        {
                            string key = item.Replace(":", "");
                            if (filter != null && filter.ContainsKey(key))
                            {
                                if (filter[key] == null)
                                {
                                    query = query.Replace(":" + key, "''");
                                }
                                else if (filter[key] is bool)
                                {
                                    bool value = (bool)filter[key];
                                    query = query.Replace(":" + key, value == true ? "1" : "0");
                                }
                                else if (filter[key] is long)
                                {
                                    long value = (long)filter[key];
                                    query = query.Replace(":" + key, value.ToString());
                                }

                                else if (filter[key] is string)
                                {
                                    string value = (string)filter[key];
                                    query = query.Replace(":" + key, string.IsNullOrWhiteSpace(value) ? "''" : "'" + value + "'");
                                }
                                else if (filter[key].GetType() == typeof(Newtonsoft.Json.Linq.JArray))
                                {
                                    processorArray(ref query, filter, key);
                                }
                            }
                            else
                            {
                                this.ReplaceBlackHole(item, ref query);
                            }
                        }
                    }

                    query = ReplaceConfigKey(query);

                    //phân tách dữ liệu tổng
                    query = Disaggregate(query);

                    var querys = ProcessSql(query);
                    foreach (var item in querys)
                    {
                        List<string> errors = new List<string>();
                        result.Add(new MOS.DAO.Sql.SqlDAO().Execute(item, ref errors) ?? new DataTable());
                        Inventec.Common.Logging.LogSystem.Info(string.Join(", ", errors));
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public void ReplaceFilter(Dictionary<string, object> filter,ref string query)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(query))
                {
                    List<string> blackHoles = GetBlackHole(query);
                    if (blackHoles != null)
                    {
                        blackHoles = blackHoles.Distinct().OrderByDescending(o => o.Length).ToList();
                        foreach (var item in blackHoles)
                        {
                            string key = item.Replace(":", "");
                            if (filter != null && filter.ContainsKey(key))
                            {
                                if (filter[key] == null)
                                {
                                    query = query.Replace(":" + key, "''");
                                }
                                else if (filter[key] is bool)
                                {
                                    bool value = (bool)filter[key];
                                    query = query.Replace(":" + key, value == true ? "1" : "0");
                                }
                                else if (filter[key] is long)
                                {
                                    long value = (long)filter[key];
                                    query = query.Replace(":" + key, value.ToString());
                                }

                                else if (filter[key] is string)
                                {
                                    string value = (string)filter[key];
                                    query = query.Replace(":" + key, string.IsNullOrWhiteSpace(value) ? "''" : "'" + value + "'");
                                }
                                else if (filter[key].GetType() == typeof(Newtonsoft.Json.Linq.JArray))
                                {
                                    processorArray(ref query, filter, key);
                                }
                            }
                            else
                            {
                                this.ReplaceBlackHole(item, ref query);
                            }
                        }
                    }

                    query = ReplaceConfigKey(query);

                    //phân tách dữ liệu tổng
                    query = Disaggregate(query);

                    
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private List<string> GetBlackHole(string query)
        {
            List<string> result = new List<string>();
            try
            {
                if (!String.IsNullOrWhiteSpace(query))
                {
                    MatchCollection match = Regex.Matches(query, ":[a-zA-Z0-9%._]+");
                    foreach (var item in match)
                    {
                        if (item != null)
                        {
                            result.Add(item.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void ReplaceBlackHole(string item, ref string query)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(query) && !String.IsNullOrWhiteSpace(item))
                {

                    query = query.Replace(item, "''");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void processorArray(ref string query, Dictionary<string, object> filter, string item)
        {
            List<JToken> value = ((JArray)filter[item]).ToList();
            if (value != null)
            {
                query = Regex.Replace(query, @"\(\s+" + ":" + item + @"\s+\)", "(" + ":" + item + ")");
                query = Regex.Replace(query,":" + item + @"\s+", "'ss' ");
            }
            int count = value.Count;
            int n = (int)((count - 1) / 1000);
            if (item.Contains("EXECUTE_ROLE_GROUP"))
            {
                List<string> str = new List<string>();
                foreach (var roleUser in value)
                {
                    MRS.MANAGER.Core.MrsReport.Lib.ForExcel.RoleUserADO r = Newtonsoft.Json.JsonConvert.DeserializeObject<MRS.MANAGER.Core.MrsReport.Lib.ForExcel.RoleUserADO>(roleUser.ToString());
                    str.Add(r.EXECUTE_ROLE_CODE + "_" + r.LOGINNAME);
                }
                query = query.Replace("':" + item + "'", "'null'").Replace(":" + item, str != null ? "'" + string.Join("','", str) + "'" : "''");
            }
            else if (value.Count <= 1000)
            {
                query = query.Replace("':" + item + "'", "'null'").Replace(":" + item, value != null ? "'" + string.Join("','", value) + "'" : "''");
            }
            else
            {

                List<JToken> valueLimit = value.Take(1000).ToList();
                query = query.Replace("':" + item + "'", "'null'").Replace(":" + item, valueLimit != null ? "'" + string.Join("','", valueLimit) + "'" : "''");
                int id = 1;
                while (value.Count > 1000)
                {
                    value = value.Skip(1000).ToList();
                    valueLimit = value.Take(value.Count > 1000 ? 1000 : value.Count).ToList();
                    string queryReplace = query.Replace(string.Format(":ADD{1}_{0}", item, id), "'" + string.Join("','", valueLimit) + "'");
                    if (queryReplace == query)
                    {
                        string genAddString = string.Format("xxxx in (:{0})", item);
                        for (int i = 0; i < n; i++)
                        {
                            genAddString += string.Format(" or xxxx in (:ADD{1}_{0})", item, i + 1);
                        }
                        throw new Exception(string.Format("ORA-01795: maximum number of expressions in a list is 1000, length of expressions in a list is {1}. \r\nPlease fix commandText from xxxx in (:{0}) to ({2})", item, count, genAddString));
                    }
                    else
                    {
                        query = queryReplace;
                    }
                    id++;
                }
            }
        }

        private string ReplaceConfigKey(string query)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(query))
                {
                    List<string> configKeys = new List<string>();
                    int ic = 0;
                    while (true)
                    {
                        ic = query.IndexOf("'<#", ic + 1);
                        if (ic < 0)
                        {
                            break;
                        }
                        int ie = query.IndexOf(";>'", ic);
                        var sql = query.Substring(ic, ie == -1 ? query.Length - ic : ie - ic);
                        if (!string.IsNullOrWhiteSpace(sql))
                        {
                            configKeys.Add(sql);
                        }

                        //query = query.Replace(sql, sql.Replace(';', ' ').Replace('\'', ' '));
                    }
                    foreach (string item in configKeys)
                    {
                        if (MRS.MANAGER.Config.Loader.dictionaryConfig != null && MRS.MANAGER.Config.Loader.dictionaryConfig.ContainsKey(item.Replace("'<#", "")))
                        {
                            if (MRS.MANAGER.Config.Loader.dictionaryConfig[item.Replace("'<#", "")] != null)
                            {
                                string value = MRS.MANAGER.Config.Loader.dictionaryConfig[item.Replace("'<#", "")].VALUE ?? MRS.MANAGER.Config.Loader.dictionaryConfig[item.Replace("'<#", "")].DEFAULT_VALUE;
                                if (!String.IsNullOrWhiteSpace(value))
                                {
                                    query = query.Replace(item + ";>'", "'" + value + "'");
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return query;
        }

        private string Disaggregate(string query)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(query))
                {
                    List<string> Aggregates = new List<string>();
                    int ic = -1;
                    while (true)
                    {
                        //'Disaggregate(Sum->trea.tdl_treatment_type_id->HIS_TREATMENT_TYPE->ID->TREATMENT_TYPE_NAME->ss.VIR_TOTAL_PRICE)'->'Disaggregate(Sum->Table->filterName->ColValue->ColDisplay->execute_loginname)'
                        ic = query.IndexOf("'Disaggregate(", ic + 1);
                        if (ic < 0)
                        {
                            break;
                        }
                        int ie = query.IndexOf(")'", ic);
                        var sql = query.Substring(ic + ("'Disaggregate(").Length, ie == -1 ? query.Length - ic : ie - ic - ("'Disaggregate(").Length);
                        if (!string.IsNullOrWhiteSpace(sql))
                        {
                            Aggregates.Add(sql);
                        }
                    }
                    //danh sách hàm
                    foreach (string item in Aggregates)
                    {
                        String[] separator = new String[] { "->" };
                        string[] statement = item.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                        if (statement.Length != 6)
                        {
                            continue;
                        }
                        string listColQuery = string.Format("select {0}||'' code,{1} name from {2}", statement[3], statement[4], statement[2]);

                        Inventec.Common.Logging.LogSystem.Info("SQL: " + listColQuery);
                        List<DataGet> listCol = new SqlDAO().GetSql<DataGet>(new CommonParam(),listColQuery);

                        //tách cột
                        List<string> disAggregates = new List<string>();
                        if (listCol != null && listCol.Count > 0)
                        {
                            foreach (var col in listCol)
                            {
                                switch (statement[0].ToLower())
                                {
                                    case "sum":
                                        disAggregates.Add(string.Format("sum(case when {0} = '{1}' then {2} else 0 end) \"COL_{3}\"", statement[1], col.CODE, statement[5],col.NAME));
                                        break;
                                    case "count":
                                        disAggregates.Add(string.Format("count(case when {0} = '{1}' then {2} else null end) \"COL_{3}\"", statement[1], col.CODE, statement[5], col.NAME));
                                        break;
                                    case "listagg":
                                        disAggregates.Add(string.Format("listagg(case when {0} = '{1}' then {2}||'; ' else null end) within group(order by null) \"COL_{3}\"", statement[1], col.CODE, statement[5], col.NAME));
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        if (disAggregates.Count > 0)
                        {
                            query = query.Replace("'Disaggregate(" + item + ")'", string.Join(",\n", disAggregates));
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return query;
        }

        private List<string> ProcessSql(string query)
        {
            List<string> result = new List<string>();
            try
            {

                if (!String.IsNullOrWhiteSpace(query))
                {
                    int ic = 0;
                    while (ic < query.Length - 1)
                    {
                        ic = query.IndexOf("--", ic + 1);
                        if (ic < 0)
                        {
                            break;
                        }
                        int ie = query.IndexOf("\n", ic);
                        var sql = query.Substring(ic, ie == -1 ? query.Length - ic : ie - ic);
                        query = query.Replace(sql, sql.Replace(';', ' ').Replace('\'', ' '));
                    }
                    ic = 0;
                    while (ic < query.Length - 1)
                    {
                        ic = query.IndexOf("/*", ic + 1);
                        if (ic < 0)
                        {
                            break;
                        }
                        int ie = query.IndexOf("*/", ic);
                        var sql = query.Substring(ic, ie == -1 ? query.Length - ic : ie - ic);
                        query = query.Replace(sql, sql.Replace(';', ' ').Replace('\'', ' '));
                    }
                    ic = 0;
                    while (ic < query.Length - 1)
                    {
                        ic = query.IndexOf(";", ic + 1);
                        if (ic < 0)
                        {
                            break;
                        }

                        var sql = query.Substring(0, ic + 1);
                        int numOfComma = sql.Count(o => o == '\'');
                        if (numOfComma % 2 == 0)
                        {
                            query = query.Remove(ic, 1);
                            query = query.Insert(ic, "_semicolon_");
                        }
                    }
                    result = query.Split(new string[] { "_semicolon_" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
            }
            catch (Exception ex)
            {
                result = new List<string>();
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
    }
}

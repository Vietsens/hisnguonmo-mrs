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
using System.Reflection;

namespace MRS.Processor.Mrs00661
{
    public partial class ManagerSql : BusinessBase
    {
        public List<HIS_TREATMENT_D> GetSum(Mrs00661Filter filter, string query)
        {
            List<HIS_TREATMENT_D> result = new List<HIS_TREATMENT_D>();
            try
            {
                PropertyInfo[] p = typeof(Mrs00661Filter).GetProperties();
                foreach (var item in p)
                {
                    if (item.PropertyType == typeof(long))
                    {
                        long value = (long)item.GetValue(filter);
                        query = query.Replace(":" + item.Name, value.ToString());
                    }
                    else if (item.PropertyType == typeof(long?))
                    {
                        long? value = (long?)item.GetValue(filter);
                        query = query.Replace(":" + item.Name, value.HasValue ? value.Value.ToString() : "''");
                    }
                    else if (item.PropertyType == typeof(string))
                    {
                        string value = (string)item.GetValue(filter);
                        query = query.Replace(":" + item.Name, string.IsNullOrWhiteSpace(value) ? "''" : value);
                    }
                    else if (item.PropertyType == typeof(List<long>))
                    {
                        List<long> value = (List<long>)item.GetValue(filter);
                        query = query.Replace(":" + item.Name, value != null ? string.Join(",", value) : "''");
                    }
                    else if (item.PropertyType == typeof(List<string>))
                    {
                        List<string> value = (List<string>)item.GetValue(filter);
                        query = query.Replace(":" + item.Name, value != null ? "'" + string.Join("','", value) + "'" : "''");
                    }
                    else if (item.PropertyType == typeof(bool?))
                    {
                        bool? value = (bool?)item.GetValue(filter);
                        query = query.Replace(":" + item.Name, value != null ? (value == true ? "1" : "0") : "''");
                    }
                }
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TREATMENT_D>(query);
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
    }
}

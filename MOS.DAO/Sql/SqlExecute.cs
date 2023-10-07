using Inventec.Common.Logging;
using Inventec.Core;
using MOS.DAO.Base;
using MOS.EFMODEL.DataModels;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOS.DAO.Sql
{
    public delegate void ParamHandler(ref object resultHolder, params OracleParameter[] parameters);

    partial class SqlExecute : EntityBase
    {
        protected const int MAX_IN_CLAUSE_SIZE = 1000;
        protected const string IN_ANCHOR = "{IN_CLAUSE}";

        //public bool ExecuteStored(ParamHandler paramHanler, ref object resultHolder, string storedProcedureSql, params OracleParameter[] parameters)
        //{
        //    bool result = false;
        //    try
        //    {
        //        if (!String.IsNullOrEmpty(storedProcedureSql))
        //        {
        //            using (var ctx = new MOS.DAO.Base.AppContext())
        //            {
        //                ((IObjectContextAdapter)ctx).ObjectContext.Connection.Open();
        //                ctx.Database.ExecuteSqlCommand(storedProcedureSql, parameters);

        //                paramHanler(ref resultHolder, parameters);

        //                ((IObjectContextAdapter)ctx).ObjectContext.Connection.Close();
        //                result = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogSystem.Error(ex);
        //        result = false;
        //    }
        //    return result;
        //}

        //public bool Execute(string sql, params object[] parameters)
        //{
        //    bool result = false;
        //    try
        //    {
        //        if (!String.IsNullOrEmpty(sql))
        //        {
        //            using (var ctx = new MOS.DAO.Base.AppContext())
        //            {
        //                ctx.Database.ExecuteSqlCommand(sql, parameters);
        //                result = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogSystem.Error(ex);
        //        result = false;
        //    }
        //    return result;
        //}

        //public bool Executes(List<string> sqls)
        //{
        //    bool result = false;
        //    string sql = "";
        //    try
        //    {
        //        if (sqls != null && sqls.Count > 0)
        //        {
        //            StringBuilder sqlBuilder = new StringBuilder();
        //            foreach (string s in sqls)
        //            {
        //                sqlBuilder.Append(string.Format("{0};", s));
        //            }
        //            sql = string.Format("BEGIN {0} END;", sqlBuilder.ToString());

        //            using (var ctx = new MOS.DAO.Base.AppContext())
        //            {
        //                ctx.Database.ExecuteSqlCommand(sql);
        //                result = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogSystem.Error(ex);
        //        LogSystem.Error("sql: " + sql);
        //        result = false;
        //    }
        //    return result;
        //}

        public List<RAW> GetSql<RAW>(string sql, params object[] parameters)
        {
            List<RAW> result = null;
            try
            {

                string entities = GetEntitiesName.Get(sql);
                if (!String.IsNullOrEmpty(sql))
                {

                    EditStatement.Replace(ref sql);

                    if (!ValidateStatement.Valid(sql))
                    {
                        return null;
                    }
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        result = ctx.Database.SqlQuery<RAW>(sql, parameters).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => sql), sql) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => parameters), parameters), LogType.Error);
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public RAW GetSqlSingle<RAW>(string sql, params object[] parameters)
        {
            RAW result = default(RAW);
            try
            {

                string entities = GetEntitiesName.Get(sql);
                if (!String.IsNullOrEmpty(sql))
                {

                    EditStatement.Replace(ref sql);

                    if (!ValidateStatement.Valid(sql))
                    {
                        return default(RAW);
                    }
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        result = ctx.Database.SqlQuery<RAW>(sql, parameters).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => sql), sql) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => parameters), parameters), LogType.Error);
                LogSystem.Error(ex);
                result = default(RAW);
            }
            return result;
        }

        /// <summary>
        /// Tra ve chuoi string co dang: IN (id1, id2, ...) or IN (id1001, id1002, ...)
        /// Luu y: trong menh de IN ko qua 1000 phan tu
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public string AddInClause(List<long> elements, string query, string property)
        {
            string inClause;
            if (IsNotNullOrEmpty(elements))
            {
                inClause = "(" + property + " IN (";
                int size = elements.Count;
                int i = 0;
                do
                {
                    size--;
                    if (++i == MAX_IN_CLAUSE_SIZE)
                    {
                        inClause += elements[size] + ")";
                        if (size > 0)
                        {
                            inClause += " OR " + property + " IN (";
                        }
                        i = 0;
                    }
                    else
                    {
                        inClause += elements[size] + ",";
                    }
                } while (size > 0);
                inClause = inClause.Substring(0, inClause.Length - 1) + "))";
            }
            else
            {
                inClause = " 1 = 0 ";
            }
            return query.Replace(IN_ANCHOR, inClause);
        }

        /**
         * Bo sung menh de NOT IN trong cau truy van
         */
        public string AddNotInClause(List<long> elements, string query, string property)
        {
            string notInClause;

            if (IsNotNullOrEmpty(elements))
            {
                notInClause = "(" + property + " NOT IN (";
                int size = elements.Count;
                int i = 0;
                do
                {
                    size--;
                    if (++i == MAX_IN_CLAUSE_SIZE)
                    {
                        notInClause = elements[size] + ")";
                        if (size > 0)
                        {
                            notInClause += " AND " + property + " NOT IN (";
                        }
                        i = 0;
                    }
                    else
                    {
                        notInClause += elements[size] + ",";
                    }
                } while (size > 0);
                notInClause = notInClause.Substring(0, notInClause.Length - 1) + "))";
            }
            else
            {
                notInClause = " 1 = 1 ";
            }
            return query.Replace(IN_ANCHOR, notInClause);
        }
        public List<RAW> GetSql<RAW>(CommonParam param, string sql, params object[] parameters)
        {
            List<RAW> result = null;
            try
            {
                if (!String.IsNullOrEmpty(sql))
                {

                    EditStatement.Replace(ref sql);

                    if (!ValidateStatement.Valid(sql))
                    {
                        return null;
                    }
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        result = ctx.Database.SqlQuery<RAW>(sql, parameters).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => sql), sql) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => parameters), parameters), LogType.Error);
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public RAW GetSqlSingle<RAW>(CommonParam param, string sql, params object[] parameters)
        {
            RAW result = default(RAW);
            try
            {
                if (!String.IsNullOrEmpty(sql))
                {

                    EditStatement.Replace(ref sql);

                    if (!ValidateStatement.Valid(sql))
                    {
                        return default(RAW);
                    }
                    using (var ctx = new MOS.DAO.Base.AppContext())
                    {
                        result = ctx.Database.SqlQuery<RAW>(sql, parameters).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => sql), sql) + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => parameters), parameters), LogType.Error);
                LogSystem.Error(ex);
                result = default(RAW);
            }
            return result;
        }
    }
}

using Inventec.Common.Logging;
using Inventec.Core;
using LIS.DAO.Base;
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

namespace LIS.DAO.Sql
{
    public delegate void ParamHandler(ref object resultHolder, params OracleParameter[] parameters);

    partial class SqlExecute : EntityBase
    {
        protected const int MAX_IN_CLAUSE_SIZE = 1000;
        protected const string IN_ANCHOR = "{IN_CLAUSE}";

        public List<RAW> GetSql<RAW>(string sql, params object[] parameters)
        {
            List<RAW> result = null;
            try
            {
                if (!String.IsNullOrEmpty(sql))
                {
                    using (var ctx = new AppContext())
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
                if (!String.IsNullOrEmpty(sql))
                {
                    using (var ctx = new AppContext())
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
    }
}

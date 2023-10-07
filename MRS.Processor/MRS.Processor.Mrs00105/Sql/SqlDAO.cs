using Inventec.Common.Repository;
using Inventec.Core;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00105
{
    public partial class SqlDAO : EntityBase
    {
        private SqlExecute SqlWorker
        {
            get
            {
                return (SqlExecute)Worker.Get<SqlExecute>();
            }
        }

        public bool ExecuteStored(ParamHandler paramHandler, ref object resultHolder, string sql, params OracleParameter[] parameters)
        {
            bool result = false;
            try
            {
                result = SqlWorker.ExecuteStored(paramHandler, ref resultHolder, sql, parameters);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        public bool Execute(string sql, params object[] parameters)
        {
            bool result = false;
            try
            {
                result = SqlWorker.Execute(sql, parameters);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        public bool Executes(List<string> sqls, ref string error)
        {
            bool result = false;
            try
            {
                result = SqlWorker.Execute(sqls, ref error);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        public List<RAW> GetSql<RAW>(string sql, params object[] parameters)
        {
            List<RAW> result = new List<RAW>();

           
                result = SqlWorker.GetSql<RAW>(sql, parameters);
          

            return result;
        }

        public RAW GetSqlSingle<RAW>(string sql, params object[] parameters)
        {
            RAW result = default(RAW);

            try
            {
                result = SqlWorker.GetSqlSingle<RAW>(sql, parameters);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
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
            return SqlWorker.AddInClause(elements, query, property);
        }

        /**
         * Bo sung menh de NOT IN trong cau truy van
         */
        public string AddNotInClause(List<long> elements, string query, string property)
        {
            return SqlWorker.AddNotInClause(elements, query, property);
        }
    }
}

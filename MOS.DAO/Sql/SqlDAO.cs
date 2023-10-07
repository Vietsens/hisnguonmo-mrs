using Inventec.Common.Repository;
using Inventec.Core;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOS.DAO.Sql
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

        //public bool ExecuteStored(ParamHandler paramHandler, ref object resultHolder, string sql, params OracleParameter[] parameters)
        //{
        //    bool result = false;
        //    try
        //    {
        //        result = SqlWorker.ExecuteStored(paramHandler, ref resultHolder, sql, parameters);
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Error(ex);
        //        result = false;
        //    }
        //    return result;
        //}

        //public bool Execute(string sql, params object[] parameters)
        //{
        //    bool result = false;
        //    try
        //    {
        //        result = SqlWorker.Execute(sql, parameters);
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Error(ex);
        //        result = false;
        //    }
        //    return result;
        //}

        //public bool Executes(List<string> sqls)
        //{
        //    bool result = false;
        //    try
        //    {
        //        result = SqlWorker.Executes(sqls);
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Error(ex);
        //        result = false;
        //    }
        //    return result;
        //}

        public List<RAW> GetSql<RAW>(string sql, params object[] parameters)
        {
            List<RAW> result = new List<RAW>();

            try
            {
                result = SqlWorker.GetSql<RAW>(sql, parameters);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

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
        public List<RAW> GetSql<RAW>(CommonParam param, string sql, params object[] parameters)
        {
            List<RAW> result = new List<RAW>();

            try
            {
                result = SqlWorker.GetSql<RAW>(param, sql, parameters);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }

        public RAW GetSqlSingle<RAW>(CommonParam param, string sql, params object[] parameters)
        {
            RAW result = default(RAW);

            try
            {
                result = SqlWorker.GetSqlSingle<RAW>(param, sql, parameters);
            }
            catch (Exception ex)
            {

                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = default(RAW);
            }

            return result;
        }

        private AnalysisDb SqlWorkerAcess
        {
            get
            {
                return (AnalysisDb)Worker.Get<AnalysisDb>();
            }
        }

        public DataTable Execute(string query, ref List<string> mess)
        {
            DataTable result = null;
            try
            {
                result = new MyAppContext().GetSqlToDataTable(query, ref mess);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public long? Count(string query)
        {
            long? result = null;
            try
            {
                result = SqlWorkerAcess.GetDataCount(query);
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
    }
}

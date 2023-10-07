using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisCashierRoom;

namespace MRS.Processor.Mrs00608
{
    public partial class Mrs00608RDOManager : BusinessBase
    {
        public Decimal? GetThenAmount(Mrs00608Filter castFilter)
        {
            try
            {
                Decimal? result = null;
                string query = "";
                query += "SELECT ";
                query += "SUM(TRA.AMOUNT) ";

                query += "FROM HIS_TRANSACTION TRA ";
                query += "JOIN HIS_TREATMENT TREA ON TRA.TREATMENT_ID = TREA.ID ";
                query += "WHERE TRA.IS_CANCEL IS NULL ";
                query += "AND TRA.TRANSACTION_TYPE_ID = 1 ";
                query += "AND (TREA.IS_ACTIVE=1 ";
                query += string.Format("OR TREA.FEE_LOCK_TIME >= {0} ", castFilter.FEE_LOCK_TIME_TO);
                query += ") ";

                query += string.Format("AND TRA.TRANSACTION_TIME < {0} ", castFilter.FEE_LOCK_TIME_TO);
                if (castFilter.TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID IN ({0}) ", string.Join(",", castFilter.TREATMENT_TYPE_IDs));
                }
                query += "ORDER BY TRA.ID ";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = DAOWorker.SqlDAO.GetSqlSingle<decimal?>(query);
                if (rs != null)
                {
                    result=(decimal)rs;
                }
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        public Decimal? GetBeforeAmount(Mrs00608Filter castFilter)
        {
            try
            {
                Decimal? result = null;
                string query = "";
                query += "SELECT ";
                query += "SUM(TRA.AMOUNT) ";

                query += "FROM HIS_TRANSACTION TRA ";
                query += "JOIN HIS_TREATMENT TREA ON TRA.TREATMENT_ID = TREA.ID ";
                query += "WHERE TRA.IS_CANCEL IS NULL ";
                query += "AND TRA.TRANSACTION_TYPE_ID = 1 ";
                query += "AND (TREA.IS_ACTIVE=1 ";
                query += string.Format("OR TREA.FEE_LOCK_TIME >= {0} ", castFilter.FEE_LOCK_TIME_FROM);
                query += ") ";

                query += string.Format("AND TRA.TRANSACTION_TIME < {0} ", castFilter.FEE_LOCK_TIME_FROM);

                if (castFilter.TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID IN ({0}) ", string.Join(",", castFilter.TREATMENT_TYPE_IDs));
                }
                query += "ORDER BY TRA.ID ";
                var rs = DAOWorker.SqlDAO.GetSqlSingle<decimal?>(query);
                if (rs != null)
                {
                    result = (decimal)rs;
                }
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        public Decimal? GetOnAmount(Mrs00608Filter castFilter)
        {
            try
            {
                Decimal? result = null;
                string query = "";
                query += "SELECT ";
                query += "SUM(TRA.AMOUNT) ";

                query += "FROM HIS_TRANSACTION TRA ";
                query += "JOIN HIS_TREATMENT TREA ON TRA.TREATMENT_ID = TREA.ID ";
                query += "WHERE TRA.IS_CANCEL IS NULL ";
                query += "AND TRA.TRANSACTION_TYPE_ID = 1 ";
                query += string.Format("AND TRA.TRANSACTION_TIME >= {0} ", castFilter.FEE_LOCK_TIME_FROM);
                query += string.Format("AND TRA.TRANSACTION_TIME < {0} ", castFilter.FEE_LOCK_TIME_TO);
                if (castFilter.TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID IN ({0}) ", string.Join(",", castFilter.TREATMENT_TYPE_IDs));
                }
                query += "ORDER BY TRA.ID ";
                var rs = DAOWorker.SqlDAO.GetSqlSingle<decimal?>(query);
                if (rs != null)
                {
                    result = (decimal)rs;
                }
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}

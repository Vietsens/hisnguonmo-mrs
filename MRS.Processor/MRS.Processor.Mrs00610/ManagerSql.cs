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

namespace MRS.Processor.Mrs00610
{
    public partial class ManagerSql : BusinessBase
    {

        //public DataTable GetByTransactionTime(Mrs00610Filter filter)
        //{
        //    List<Mrs00610RDO> result = new List<Mrs00610RDO>();
        //    try
        //    {
        //        string query = "";
        //        query += "SELECT ";
        //        query += "SS.AMOUNT, ";
        //        query += "SS.VIR_TOTAL_PRICE, ";
        //        query += "SS.SERVICE_ID, ";
        //        query += "SS.TDL_SERVICE_CODE, ";
        //        query += "SS.TDL_SERVICE_NAME, ";
        //        query += "0 AS IN_AMOUNT, ";
        //        query += "0 AS OUT_AMOUNT, ";
        //        query += "SS.AMOUNT AS ALL_AMOUNT, ";
        //        query += "SRC.CATEGORY_CODE AS CATEGORY_CODE, ";
        //        query += "SRC.CATEGORY_NAME AS CATEGORY_NAME, ";
        //        query += "SRC.SERVICE_TYPE_CODE AS SERVICE_TYPE_CODE, ";
        //        query += "SRC.SERVICE_TYPE_NAME AS SERVICE_TYPE_NAME, ";
        //        query += "(CASE WHEN TRAN.TRANSACTION_TIME IS NOT NULL THEN TRAN.TRANSACTION_TIME ELSE TREA.FEE_LOCK_TIME END) AS TIME, ";
        //        query += "TRAN.TRANSACTION_CODE AS TRANSACTION_CODE, ";
        //        query += "0 AS IN_TOTAL_PRICE, ";
        //        query += "0 AS OUT_TOTAL_PRICE, ";
        //        query += "SS.VIR_TOTAL_PRICE AS ALL_TOTAL_PRICE ";
        //        query += "FROM HIS_RS.HIS_SERE_SERV SS ";
        //        query += "LEFT JOIN HIS_RS.HIS_TRANSACTION TRAN ON (TRAN.TREATMENT_ID = SS.TDL_TREATMENT_ID AND TRAN.IS_CANCEL IS NULL and tran.transaction_type_id in (1,3)  AND not exists(select 1 from his_rs.his_transaction where is_cancel is null and tran.treatment_id = treatment_id  and (transaction_time>tran.transaction_time or (transaction_time=tran.transaction_time and id>tran.id)) and transaction_type_id in (1,3) and id<>tran.id) ";
        //        if (filter.TRANSACTION_TIME_FROM != null)
        //        {
        //            query += string.Format("AND TRAN.TRANSACTION_TIME >= {0} ", filter.TRANSACTION_TIME_FROM);
        //        }
        //        if (filter.TRANSACTION_TIME_TO != null)
        //        {
        //            query += string.Format("AND TRAN.TRANSACTION_TIME < {0} ", filter.TRANSACTION_TIME_TO);
        //        }
        //        query += ") ";
        //        query += "LEFT JOIN HIS_RS.HIS_TREATMENT TREA ON (TREA.ID = SS.TDL_TREATMENT_ID AND TREA.IS_ACTIVE = 0 AND not exists(select 1 from his_rs.his_transaction where is_cancel is null and treatment_id = trea.id) ";
        //        if (filter.TRANSACTION_TIME_FROM != null)
        //        {
        //            query += string.Format("AND TREA.FEE_LOCK_TIME >= {0} ", filter.TRANSACTION_TIME_FROM);
        //        }
        //        if (filter.TRANSACTION_TIME_TO != null)
        //        {
        //            query += string.Format("AND TREA.FEE_LOCK_TIME < {0} ", filter.TRANSACTION_TIME_TO);
        //        }
        //        query += "), ";
        //        query += "HIS_RS.V_HIS_SERVICE_RETY_CAT SRC ";
        //        query += "WHERE 1=1 ";
        //        query += "AND SS.IS_DELETE =0 AND SS.IS_NO_EXECUTE IS NULL AND SS.IS_EXPEND IS NULL AND SS.SERVICE_REQ_ID IS NOT NULL AND (TREA.ID IS NOT NULL OR TRAN.ID IS NOT NULL) ";
        //        query += "AND SRC.SERVICE_ID = SS.SERVICE_ID AND SRC.REPORT_TYPE_CODE = 'MRS00610' ";

        //        if (filter.REPORT_TYPE_CAT_ID != null)
        //        {
        //            query += string.Format("AND SRC.REPORT_TYPE_CAT_ID = {0} ", filter.REPORT_TYPE_CAT_ID);
        //        }

        //        if (filter.REPORT_TYPE_CAT_IDs != null)
        //        {
        //            query += string.Format("AND SRC.REPORT_TYPE_CAT_ID IN ({0}) ", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
        //        }
        //        Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
        //        var rs = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00610RDO>(query);
        //        if (rs != null)
        //        {
        //            result.AddRange(rs);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Error(ex);
        //        result = null;
        //    }
        //    return result;
        //}
        public List<Mrs00610RDO> GetByTransactionTime(Mrs00610Filter filter)
        {
            List<Mrs00610RDO> result = new List<Mrs00610RDO>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "SS.SERVICE_ID, ";
                query += "SS.TDL_SERVICE_CODE, ";
                query += "SS.TDL_SERVICE_NAME, ";
                query += "SRC.CATEGORY_CODE AS CATEGORY_CODE, ";
                query += "SRC.CATEGORY_NAME AS CATEGORY_NAME, ";
                query += "SRC.SERVICE_TYPE_CODE AS SERVICE_TYPE_CODE, ";
                query += "SRC.SERVICE_TYPE_NAME AS SERVICE_TYPE_NAME, ";
                query += "SUM(CASE WHEN TREA1.TDL_TREATMENT_TYPE_ID =3 THEN SS.AMOUNT ELSE 0 END) AS IN_AMOUNT, ";
                query += "SUM(CASE WHEN TREA1.TDL_TREATMENT_TYPE_ID <>3 THEN SS.AMOUNT ELSE 0 END) AS OUT_AMOUNT, ";
                query += "SUM(SS.AMOUNT) AS ALL_AMOUNT, ";
                query += "SUM(CASE WHEN TREA1.TDL_TREATMENT_TYPE_ID =3 THEN SS.VIR_TOTAL_PRICE ELSE 0 END) AS IN_TOTAL_PRICE, ";
                query += "SUM(CASE WHEN TREA1.TDL_TREATMENT_TYPE_ID <>3 THEN SS.VIR_TOTAL_PRICE ELSE 0 END) AS OUT_TOTAL_PRICE, ";
                query += "SUM(SS.VIR_TOTAL_PRICE) AS ALL_TOTAL_PRICE ";
                query += "FROM HIS_RS.HIS_SERE_SERV SS ";

                query += "JOIN HIS_RS.HIS_TREATMENT TREA1 ON TREA1.ID = SS.TDL_TREATMENT_ID ";
                query += "LEFT JOIN HIS_RS.HIS_TRANSACTION TRAN ON (TRAN.TREATMENT_ID = SS.TDL_TREATMENT_ID AND TRAN.IS_CANCEL IS NULL and tran.transaction_type_id in (1,3)  AND not exists(select 1 from his_rs.his_transaction where is_cancel is null and tran.treatment_id = treatment_id  and (transaction_time>tran.transaction_time or (transaction_time=tran.transaction_time and id>tran.id)) and transaction_type_id in (1,3) and id<>tran.id) ";
                if (filter.TRANSACTION_TIME_FROM != null)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME >= {0} ", filter.TRANSACTION_TIME_FROM);
                }
                if (filter.TRANSACTION_TIME_TO != null)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME < {0} ", filter.TRANSACTION_TIME_TO);
                }
                query += ") ";
                query += "LEFT JOIN HIS_RS.HIS_TREATMENT TREA ON (TREA.ID = SS.TDL_TREATMENT_ID AND TREA.IS_ACTIVE = 0 AND not exists(select 1 from his_rs.his_transaction where is_cancel is null and treatment_id = trea.id) ";
                if (filter.TRANSACTION_TIME_FROM != null)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME >= {0} ", filter.TRANSACTION_TIME_FROM);
                }
                if (filter.TRANSACTION_TIME_TO != null)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME < {0} ", filter.TRANSACTION_TIME_TO);
                }
                query += "), ";
                query += "HIS_RS.V_HIS_SERVICE_RETY_CAT SRC ";
                query += "WHERE 1=1 ";
                query += "AND SS.IS_DELETE =0 AND SS.IS_NO_EXECUTE IS NULL AND SS.IS_EXPEND IS NULL AND SS.SERVICE_REQ_ID IS NOT NULL AND (TREA.ID IS NOT NULL OR TRAN.ID IS NOT NULL) ";
                query += "AND SRC.SERVICE_ID = SS.SERVICE_ID AND SRC.REPORT_TYPE_CODE = 'MRS00610' ";

                if (filter.REPORT_TYPE_CAT_ID != null)
                {
                    query += string.Format("AND SRC.REPORT_TYPE_CAT_ID = {0} ", filter.REPORT_TYPE_CAT_ID);
                }

                if (filter.REPORT_TYPE_CAT_IDs != null)
                {
                    query += string.Format("AND SRC.REPORT_TYPE_CAT_ID IN ({0}) ", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                }
                query += "group by  "; 
                query += "SS.SERVICE_ID, ";
                query += "SS.TDL_SERVICE_CODE, ";
                query += "SS.TDL_SERVICE_NAME, ";
                query += "SRC.CATEGORY_CODE, ";
                query += "SRC.CATEGORY_NAME, ";
                //query += "trea1.TDL_TREATMENT_TYPE_ID, ";
                query += "SRC.SERVICE_TYPE_CODE, ";
                query += "SRC.SERVICE_TYPE_NAME ";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00610RDO>(query);
                if (rs != null)
                {
                    result.AddRange(rs);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<HIS_TREATMENT> GetTreatmentBill(Mrs00610Filter filter)
        {
            List<HIS_TREATMENT> result = new List<HIS_TREATMENT>();
            try
            {
                string query = "";
            query = "";
            query += "SELECT ";
            query += "TREA.* ";

            query += "FROM HIS_RS.HIS_TREATMENT TREA ";
            query += "LEFT JOIN HIS_RS.HIS_TRANSACTION TRAN ON (TRAN.TREATMENT_ID = TREA.ID AND TRAN.IS_CANCEL IS NULL ";
            if (filter.TRANSACTION_TIME_FROM != null)
            {
                query += string.Format("AND TRAN.TRANSACTION_TIME >= {0} ", filter.TRANSACTION_TIME_FROM);
            }
            if (filter.TRANSACTION_TIME_TO != null)
            {
                query += string.Format("AND TRAN.TRANSACTION_TIME < {0} ", filter.TRANSACTION_TIME_TO);
            }
            query += ") ";

            query += "WHERE 1=1 ";
            query += "AND (TRAN.ID IS NOT NULL OR (TREA.IS_ACTIVE = 0 ";
            if (filter.TRANSACTION_TIME_FROM != null)
            {
                query += string.Format("AND TREA.FEE_LOCK_TIME >= {0} ", filter.TRANSACTION_TIME_FROM);
            }
            if (filter.TRANSACTION_TIME_TO != null)
            {
                query += string.Format("AND TREA.FEE_LOCK_TIME < {0} ", filter.TRANSACTION_TIME_TO);
            }

            query += ")) ";
           
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            var Trea = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TREATMENT>(query);

            if (Trea != null)
            {
                result = Trea;
            }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<Mrs00610RDO> GetByFeeLockTime(Mrs00610Filter filter)
        {
            List<Mrs00610RDO> result = new List<Mrs00610RDO>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "SS.*, ";
                query += "(CASE WHEN TREA.TDL_TREATMENT_TYPE_ID=3 THEN SS.AMOUNT ELSE 0 END) AS IN_AMOUNT, ";
                query += "(CASE WHEN TREA.TDL_TREATMENT_TYPE_ID=3 THEN 0 ELSE SS.AMOUNT END) AS OUT_AMOUNT, ";
                query += "SS.AMOUNT AS ALL_AMOUNT, ";
                query += "SRC.CATEGORY_CODE AS CATEGORY_CODE, ";
                query += "SRC.CATEGORY_NAME AS CATEGORY_NAME, ";
                query += "TREA.FEE_LOCK_TIME AS TIME, ";
                query += "(CASE WHEN TREA.TDL_TREATMENT_TYPE_ID=3 THEN NVL(SS.VIR_TOTAL_PRICE,0) ELSE 0 END) AS IN_TOTAL_PRICE, ";
                query += "(CASE WHEN TREA.TDL_TREATMENT_TYPE_ID=3 THEN 0 ELSE NVL(SS.VIR_TOTAL_PRICE,0) END) AS OUT_TOTAL_PRICE, ";
                query += "NVL(SS.VIR_TOTAL_PRICE,0) AS ALL_TOTAL_PRICE ";

                query += "FROM HIS_SERE_SERV SS, ";
                query += "HIS_TREATMENT TREA, ";
                query += "V_HIS_SERVICE_RETY_CAT SRC ";
                query += "WHERE 1=1 ";
                query += "AND SS.IS_DELETE =0 AND SS.IS_NO_EXECUTE IS NULL AND SS.IS_EXPEND IS NULL AND SS.SERVICE_REQ_ID IS NOT NULL ";
                query += "AND TREA.ID = SS.TDL_TREATMENT_ID AND TREA.IS_ACTIVE =0 ";
                query += "AND SRC.SERVICE_ID = SS.SERVICE_ID AND SRC.REPORT_TYPE_CODE = 'MRS00610' ";

                if (filter.FEE_LOCK_TIME_FROM != null)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME >= {0} ", filter.FEE_LOCK_TIME_FROM);
                }
                if (filter.FEE_LOCK_TIME_TO != null)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME < {0} ", filter.FEE_LOCK_TIME_TO);
                }

                if (filter.REPORT_TYPE_CAT_ID != null)
                {
                    query += string.Format("AND SRC.REPORT_TYPE_CAT_ID = {0} ", filter.REPORT_TYPE_CAT_ID);
                }

                if (filter.REPORT_TYPE_CAT_IDs != null)
                {
                    query += string.Format("AND SRC.REPORT_TYPE_CAT_ID IN ({0}) ", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00610RDO>(query);
                if (rs != null)
                {
                    result.AddRange(rs);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        internal List<HIS_TREATMENT> GetTreatmentFeeLock(Mrs00610Filter filter)
        {
            List<HIS_TREATMENT> result = new List<HIS_TREATMENT>();
            try
            {
                string query = "";
                query = "";
                query += "SELECT ";
                query += "TREA.* ";

                query += "FROM HIS_RS.HIS_TREATMENT TREA ";

                query += "WHERE 1=1 ";
                query += "AND TREA.IS_ACTIVE =0 ";
                if (filter.FEE_LOCK_TIME_FROM != null)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME >= {0} ", filter.TRANSACTION_TIME_FROM);
                }
                if (filter.FEE_LOCK_TIME_FROM != null)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME < {0} ", filter.TRANSACTION_TIME_TO);
                }

                query += ")) ";

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var Trea = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TREATMENT>(query);

                if (Trea != null)
                {
                    result = Trea;
                }
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

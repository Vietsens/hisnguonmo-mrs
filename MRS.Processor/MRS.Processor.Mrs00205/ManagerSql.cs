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

namespace MRS.Processor.Mrs00205
{
    public partial class ManagerSql : BusinessBase
    {
        public Mrs00205GDO GetByBillTime(Mrs00205Filter filter)
        {
            Mrs00205GDO result = new Mrs00205GDO();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "SS.* ";

                query += "FROM HIS_RS.HIS_SERE_SERV SS ";
                query += "LEFT JOIN HIS_RS.HIS_TRANSACTION TRAN ON (TRAN.TREATMENT_ID = SS.TDL_TREATMENT_ID AND TRAN.IS_CANCEL IS NULL  AND TRAN.SALE_TYPE_ID IS NULL and tran.transaction_type_id in (1,3) AND not exists(select 1 from his_rs.his_transaction where is_cancel is null   AND SALE_TYPE_ID IS NULL and tran.treatment_id = treatment_id and (transaction_time>tran.transaction_time or (transaction_time=tran.transaction_time and id>tran.id)) and transaction_type_id in (1,3) and id<>tran.id) ";
                if (filter.TRANSACTION_TIME_FROM != null)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME >= {0} ", filter.TRANSACTION_TIME_FROM);
                }
                if (filter.TRANSACTION_TIME_TO != null)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME < {0} ", filter.TRANSACTION_TIME_TO);
                }
                query += ") ";

                query += "LEFT JOIN HIS_RS.HIS_TREATMENT TREA ON (TREA.ID = SS.TDL_TREATMENT_ID AND TREA.IS_ACTIVE = 0 AND not exists(select 1 from his_rs.his_transaction where is_cancel is null  AND SALE_TYPE_ID IS NULL  and treatment_id = trea.id) ";
                if (filter.TRANSACTION_TIME_FROM != null)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME >= {0} ", filter.TRANSACTION_TIME_FROM);
                }
                if (filter.TRANSACTION_TIME_TO != null)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME < {0} ", filter.TRANSACTION_TIME_TO);
                }
               
                query += ") ";
               
                query += "WHERE 1=1 ";
                query += "AND SS.IS_DELETE =0 AND SS.IS_NO_EXECUTE IS NULL AND SS.IS_EXPEND IS NULL AND SS.SERVICE_REQ_ID IS NOT NULL AND (TREA.ID IS NOT NULL OR TRAN.ID IS NOT NULL) ";
               
                if (filter.SERVICE_TYPE_ID != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID = {0} ", filter.SERVICE_TYPE_ID);
                }

                if (filter.SERVICE_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID IN ({0}) ", string.Join(",", filter.SERVICE_TYPE_IDs));
                }

                if (filter.PATIENT_TYPE_ID != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID = {0} ", filter.PATIENT_TYPE_ID);
                }

                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID IN ({0}) ", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (filter.TREAT_PATIENT_TYPE_ID != null)
                {
                    query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID = {0} ", filter.TREAT_PATIENT_TYPE_ID);
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERE_SERV>(query);

                if (rs != null)
                {
                    result.HIS_SERE_SERV = rs;
                }
               
                query = "";
                query += "SELECT ";
                query += "TRAN.* ";

                query += "FROM HIS_RS.HIS_TRANSACTION TRAN ";

                query += "WHERE 1=1 ";
                query += "AND TRAN.IS_CANCEL IS NULL  AND TRAN.SALE_TYPE_ID IS NULL  and tran.transaction_type_id in (1,3) AND not exists(select 1 from his_rs.his_transaction where is_cancel is null   AND SALE_TYPE_ID IS NULL and tran.treatment_id = treatment_id and (transaction_time>tran.transaction_time or (transaction_time=tran.transaction_time and id>tran.id)) and transaction_type_id in (1,3) and id<>tran.id) ";
                if (filter.TRANSACTION_TIME_FROM != null)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME >= {0} ", filter.TRANSACTION_TIME_FROM);
                }
                if (filter.TRANSACTION_TIME_TO != null)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME < {0} ", filter.TRANSACTION_TIME_TO);
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var tran = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TRANSACTION>(query);

                if (tran != null)
                {
                    result.HIS_TRANSACTION = tran;
                }

                query = "";
                query += "SELECT ";
                query += "TREA.* ";

                query += "FROM HIS_RS.HIS_TREATMENT TREA ";
              
                query += "LEFT JOIN HIS_RS.HIS_TRANSACTION TRAN ON (TRAN.TREATMENT_ID = TREA.ID AND TRAN.IS_CANCEL IS NULL  AND TRAN.SALE_TYPE_ID IS NULL  ";
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
                query += "AND TRAN.ID IS NOT NULL ";
                //query += "AND (TRAN.ID IS NOT NULL OR (TREA.IS_ACTIVE = 0 ";
                //if (filter.TRANSACTION_TIME_FROM != null)
                //{
                //    query += string.Format("AND TREA.FEE_LOCK_TIME >= {0} ", filter.TRANSACTION_TIME_FROM);
                //}
                //if (filter.TRANSACTION_TIME_TO != null)
                //{
                //    query += string.Format("AND TREA.FEE_LOCK_TIME < {0} ", filter.TRANSACTION_TIME_TO);
                //}

                //query += ")) ";
                if (filter.TREAT_PATIENT_TYPE_ID != null)
                {
                    query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID = {0} ", filter.TREAT_PATIENT_TYPE_ID);
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var Trea = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TREATMENT>(query);

                if (Trea != null)
                {
                    result.HIS_TREATMENT = Trea;
                }
                

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }


        public Mrs00205GDO GetByIntructionTime(Mrs00205Filter filter)
        {
            Mrs00205GDO result = new Mrs00205GDO();
            try
            {
                
                #region Ngoai tru van lay thanh toan
                string query = "";
                query += "SELECT ";
                query += "SS.* ";

                query += "FROM HIS_RS.HIS_SERE_SERV SS ";
                query += "JOIN HIS_RS.HIS_TREATMENT TREA1 ON (TREA1.ID = SS.TDL_TREATMENT_ID AND TREA1.TDL_TREATMENT_TYPE_ID <3) ";
                
                query += "LEFT JOIN HIS_RS.HIS_TRANSACTION TRAN ON (TRAN.TREATMENT_ID = SS.TDL_TREATMENT_ID AND TRAN.IS_CANCEL IS NULL  AND TRAN.SALE_TYPE_ID IS NULL  and tran.transaction_type_id in (1,3) AND not exists(select 1 from his_rs.his_transaction where is_cancel is null  AND SALE_TYPE_ID IS NULL  and tran.treatment_id = treatment_id and (transaction_time>tran.transaction_time or (transaction_time=tran.transaction_time and id>tran.id)) and transaction_type_id in (1,3) and id<>tran.id) ";
                if (filter.TRANSACTION_TIME_FROM != null)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME >= {0} ", filter.TRANSACTION_TIME_FROM);
                }
                if (filter.TRANSACTION_TIME_TO != null)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME < {0} ", filter.TRANSACTION_TIME_TO);
                }
                query += ") ";

                query += "LEFT JOIN HIS_RS.HIS_TREATMENT TREA ON (TREA.ID = SS.TDL_TREATMENT_ID AND TREA.IS_ACTIVE = 0 AND not exists(select 1 from his_rs.his_transaction where is_cancel is null   AND SALE_TYPE_ID IS NULL and treatment_id = trea.id) ";
                if (filter.TRANSACTION_TIME_FROM != null)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME >= {0} ", filter.TRANSACTION_TIME_FROM);
                }
                if (filter.TRANSACTION_TIME_TO != null)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME < {0} ", filter.TRANSACTION_TIME_TO);
                }
                query += ") ";
                query += "WHERE 1=1 ";
                query += "AND SS.IS_DELETE =0 AND SS.IS_NO_EXECUTE IS NULL AND SS.IS_EXPEND IS NULL AND SS.SERVICE_REQ_ID IS NOT NULL AND (TREA.ID IS NOT NULL OR TRAN.ID IS NOT NULL) ";

                if (filter.SERVICE_TYPE_ID != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID = {0} ", filter.SERVICE_TYPE_ID);
                }

                if (filter.SERVICE_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID IN ({0}) ", string.Join(",", filter.SERVICE_TYPE_IDs));
                }

                if (filter.PATIENT_TYPE_ID != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID = {0} ", filter.PATIENT_TYPE_ID);
                }

                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID IN ({0}) ", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (filter.TREAT_PATIENT_TYPE_ID != null)
                {
                    query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID = {0} ", filter.TREAT_PATIENT_TYPE_ID);
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERE_SERV>(query);

                if (rs != null)
                {
                    result.HIS_SERE_SERV = rs;
                }

                query = "";
                query += "SELECT ";
                query += "TRAN.* ";

                query += "FROM HIS_RS.HIS_TRANSACTION TRAN ";
                query += "JOIN HIS_RS.HIS_TREATMENT TREA1 ON (TREA1.ID = TRAN.TREATMENT_ID AND TREA1.TDL_TREATMENT_TYPE_ID <3) ";
                
               
                query += "WHERE 1=1 ";
                query += "AND TRAN.IS_CANCEL IS NULL  AND TRAN.SALE_TYPE_ID IS NULL  and tran.transaction_type_id in (1,3) AND not exists(select 1 from his_rs.his_transaction where is_cancel is null  AND SALE_TYPE_ID IS NULL  and tran.treatment_id = treatment_id and (transaction_time>tran.transaction_time or (transaction_time=tran.transaction_time and id>tran.id)) and transaction_type_id in (1,3) and id<>tran.id) ";
                if (filter.TRANSACTION_TIME_FROM != null)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME >= {0} ", filter.TRANSACTION_TIME_FROM);
                }
                if (filter.TRANSACTION_TIME_TO != null)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME < {0} ", filter.TRANSACTION_TIME_TO);
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var tran = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TRANSACTION>(query);

                if (tran != null)
                {
                    result.HIS_TRANSACTION = tran;
                }

                query = "";
                query += "SELECT ";
                query += "TREA.* ";

                query += "FROM HIS_RS.HIS_TREATMENT TREA ";
                
                query += "LEFT JOIN HIS_RS.HIS_TRANSACTION TRAN ON (TRAN.TREATMENT_ID = TREA.ID AND TRAN.IS_CANCEL IS NULL  AND TRAN.SALE_TYPE_ID IS NULL  ";
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
                query += "AND TREA.TDL_TREATMENT_TYPE_ID<3 ";
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
                if (filter.TREAT_PATIENT_TYPE_ID != null)
                {
                    query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID = {0} ", filter.TREAT_PATIENT_TYPE_ID);
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var Trea = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TREATMENT>(query);

                if (Trea != null)
                {
                    result.HIS_TREATMENT = Trea;
                }
                #endregion
                #region Noi tru lay theo thoi gian chi dinh
                query = "";
                query += "SELECT ";
                query += "SS.* ";

                query += "FROM HIS_RS.HIS_SERE_SERV SS ";
                query += "JOIN HIS_RS.HIS_TREATMENT TREA ON (TREA.ID = SS.TDL_TREATMENT_ID AND TREA.TDL_TREATMENT_TYPE_ID =3) ";
               
                
                query += "WHERE 1=1 ";
                query += "AND SS.IS_DELETE =0 AND SS.IS_NO_EXECUTE IS NULL AND SS.IS_EXPEND IS NULL AND SS.SERVICE_REQ_ID IS NOT NULL ";
                if (filter.TRANSACTION_TIME_FROM != null)
                {
                    query += string.Format("AND SS.TDL_INTRUCTION_TIME >= {0} ", filter.TRANSACTION_TIME_FROM);
                }
                if (filter.TRANSACTION_TIME_TO != null)
                {
                    query += string.Format("AND SS.TDL_INTRUCTION_TIME < {0} ", filter.TRANSACTION_TIME_TO);
                }

                if (filter.SERVICE_TYPE_ID != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID = {0} ", filter.SERVICE_TYPE_ID);
                }

                if (filter.SERVICE_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID IN ({0}) ", string.Join(",", filter.SERVICE_TYPE_IDs));
                }

                if (filter.PATIENT_TYPE_ID != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID = {0} ", filter.PATIENT_TYPE_ID);
                }

                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID IN ({0}) ", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (filter.TREAT_PATIENT_TYPE_ID != null)
                {
                    query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID = {0} ", filter.TREAT_PATIENT_TYPE_ID);
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rsIn = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERE_SERV>(query);

                if (rsIn != null)
                {
                    result.HIS_SERE_SERV.AddRange(rsIn);
                }

                query = "";
                query += "SELECT ";
                query += "TRAN.* ";
                
                query += "FROM HIS_RS.HIS_SERE_SERV SS ";
                query += "JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.TREATMENT_ID = SS.TDL_TREATMENT_ID ";
                query += "JOIN HIS_RS.HIS_TREATMENT TREA ON (TREA.ID = SS.TDL_TREATMENT_ID AND TREA.TDL_TREATMENT_TYPE_ID =3) ";
                
                
                query += "WHERE 1=1 ";
                query += "AND SS.IS_DELETE =0 AND SS.IS_NO_EXECUTE IS NULL AND SS.IS_EXPEND IS NULL AND SS.SERVICE_REQ_ID IS NOT NULL ";
                if (filter.TRANSACTION_TIME_FROM != null)
                {
                    query += string.Format("AND SS.TDL_INTRUCTION_TIME >= {0} ", filter.TRANSACTION_TIME_FROM);
                }
                if (filter.TRANSACTION_TIME_TO != null)
                {
                    query += string.Format("AND SS.TDL_INTRUCTION_TIME < {0} ", filter.TRANSACTION_TIME_TO);
                }

                if (filter.SERVICE_TYPE_ID != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID = {0} ", filter.SERVICE_TYPE_ID);
                }

                if (filter.SERVICE_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID IN ({0}) ", string.Join(",", filter.SERVICE_TYPE_IDs));
                }

                if (filter.PATIENT_TYPE_ID != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID = {0} ", filter.PATIENT_TYPE_ID);
                }

                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID IN ({0}) ", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (filter.TREAT_PATIENT_TYPE_ID != null)
                {
                    query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID = {0} ", filter.TREAT_PATIENT_TYPE_ID);
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var tranIn = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TRANSACTION>(query);

                if (tranIn != null)
                {
                    result.HIS_TRANSACTION.AddRange(tranIn);
                }


                query = "";
                query += "SELECT ";
                query += "TREA.* ";

                query += "FROM HIS_RS.HIS_SERE_SERV SS ";
                query += "JOIN HIS_RS.HIS_TREATMENT TREA ON (TREA.ID = SS.TDL_TREATMENT_ID AND TREA.TDL_TREATMENT_TYPE_ID =3) ";
           
                query += "WHERE 1=1 ";
                query += "AND SS.IS_DELETE =0 AND SS.IS_NO_EXECUTE IS NULL AND SS.IS_EXPEND IS NULL AND SS.SERVICE_REQ_ID IS NOT NULL ";
                if (filter.TRANSACTION_TIME_FROM != null)
                {
                    query += string.Format("AND SS.TDL_INTRUCTION_TIME >= {0} ", filter.TRANSACTION_TIME_FROM);
                }
                if (filter.TRANSACTION_TIME_TO != null)
                {
                    query += string.Format("AND SS.TDL_INTRUCTION_TIME < {0} ", filter.TRANSACTION_TIME_TO);
                }

                if (filter.SERVICE_TYPE_ID != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID = {0} ", filter.SERVICE_TYPE_ID);
                }

                if (filter.SERVICE_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID IN ({0}) ", string.Join(",", filter.SERVICE_TYPE_IDs));
                }

                if (filter.PATIENT_TYPE_ID != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID = {0} ", filter.PATIENT_TYPE_ID);
                }

                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID IN ({0}) ", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (filter.TREAT_PATIENT_TYPE_ID != null)
                {
                    query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID = {0} ", filter.TREAT_PATIENT_TYPE_ID);
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var TreaIn = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TREATMENT>(query);

                if (TreaIn != null)
                {
                    result.HIS_TREATMENT.AddRange(TreaIn);
                }
                #endregion
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

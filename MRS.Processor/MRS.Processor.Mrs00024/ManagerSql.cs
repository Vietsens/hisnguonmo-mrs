using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00024
{
    class ManagerSql
    {
        internal static List<Mrs00024RDO> GetSereServ(Mrs00024Filter filter)
        {
            List<Mrs00024RDO> result = null;
            try
            {
                string query = "";
                query += "SELECT \n";
                query += "TREA.PATIENT_ID, \n";
                query += "TREA.TDL_PATIENT_CODE AS PATIENT_CODE, \n";
                query += "TREA.TDL_PATIENT_NAME AS PATIENT_NAME, \n";
                query += "SR.CONSULTANT_LOGINNAME, \n";
                query += "SR.CONSULTANT_USERNAME, \n";
                query += "SS.*, \n";
                query += "SS.TDL_SERVICE_CODE AS SERVICE_CODE, \n";
                query += "SS.TDL_SERVICE_NAME AS SERVICE_NAME, \n";
                query += "SVT.SERVICE_TYPE_CODE AS SERVICE_TYPE_CODE, \n";
                query += "SVT.SERVICE_TYPE_NAME AS SERVICE_TYPE_NAME \n";
                query += "FROM V_HIS_SERE_SERV SS \n";
                query += "LEFT JOIN HIS_SERE_SERV_BILL SSB ON SS.ID = SSB.SERE_SERV_ID \n";
                query += "LEFT JOIN (SELECT ID,CONSULTANT_LOGINNAME,CONSULTANT_USERNAME from HIS_SERVICE_REQ) SR ON SR.ID = SS.SERVICE_REQ_ID  \n";
                query += "LEFT JOIN HIS_SERVICE_TYPE SVT ON SVT.ID  = SS.TDL_SERVICE_TYPE_ID \n";
                query += "JOIN HIS_TREATMENT TREA ON SS.TDL_TREATMENT_ID = TREA.ID \n";
                query += "LEFT JOIN HIS_TRANSACTION TRAN ON SSB.BILL_ID = TRAN.ID \n";
                //query += "LEFT JOIN HIS_EXECUTE_ROOM ER ON ER.ROOM_ID = SS.TDL_EXECUTE_ROOM_ID \n";
                query += "WHERE 1=1 \n";

                if (filter.SERVICE_TYPE_IDs!=null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID in ({0}) \n", string.Join(",",filter.SERVICE_TYPE_IDs));
                }

                if (filter.PATIENT_TYPE_ID.HasValue)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID = {0} \n", filter.PATIENT_TYPE_ID.Value);
                }

                if (filter.TREATMENT_TYPE_ID.HasValue)
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID = {0} \n", filter.TREATMENT_TYPE_ID.Value);
                }
                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID in ({0}) \n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }
                if (filter.REQUEST_ROOM_IDs != null)
                {
                    query += string.Format("AND SS.TDL_REQUEST_ROOM_ID in ({0}) \n", string.Join(",", filter.REQUEST_ROOM_IDs));
                }
                if (filter.EXACT_EXECUTE_ROOM_IDs != null)
                {
                    query += string.Format("AND SS.TDL_EXECUTE_ROOM_ID in (select room_id from his_execute_room where ID in ({0})) \n", string.Join(",", filter.EXACT_EXECUTE_ROOM_IDs));
                }

                if (filter.CHOOSE_TIME==true)
                {
                    query += "AND TREA.IS_ACTIVE = 0 \n";
                    query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else
                {
                    query += "AND TRAN.ID > 0 \n";
                    query += "AND TRAN.IS_CANCEL IS NULL \n";
                    query += string.Format("AND TRAN.TRANSACTION_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00024RDO>(query);
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

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

namespace MRS.Processor.Mrs00354
{
     class ManagerSql
    {

        public List<DataGet> GetRdo(Mrs00354Filter filter)
        {
            try
            {
                List<DataGet> result = new List<DataGet>();
                string query = "-- from Qcs\n";
                query += "SELECT \n";
                query += "SS.ID, \n";//
                query += "SS.TDL_TREATMENT_ID, \n";//
                query += "TREA.BRANCH_ID, \n";//
                query += "SS.IS_NO_EXECUTE, \n";//
                query += "SS.IS_NO_PAY, \n";//
                query += "TREA.TDL_TREATMENT_TYPE_ID, \n";//
                query += "SS.TDL_SERVICE_CODE, \n";//
                query += "SS.VIR_TOTAL_PRICE, \n";//
                query += "SS.VIR_TOTAL_PATIENT_PRICE, \n";//
                query += "SS.TDL_SERVICE_TYPE_ID, \n";//
                query += "SS.TDL_REQUEST_ROOM_ID, \n";//
                query += "SS.TDL_EXECUTE_ROOM_ID, \n";
                query += "SS.TDL_EXECUTE_DEPARTMENT_ID, \n";
                query += "SS.TDL_REQUEST_DEPARTMENT_ID, \n";
                query += "SS.MEDICINE_ID, \n";
                query += "SS.PATIENT_TYPE_ID, \n";
                query += "TREA.TDL_PATIENT_TYPE_ID, \n";
                query += "CASE WHEN TREA.END_DEPARTMENT_ID IS NOT NULL THEN TREA.END_DEPARTMENT_ID ELSE TREA.LAST_DEPARTMENT_ID END AS END_DEPARTMENT_ID, \n";
                query += "TREA.END_ROOM_ID, \n";
                query += "SSB.PRICE BILL_PRICE, \n";
                query += "SS.VIR_TOTAL_HEIN_PRICE, \n";
              
                query += "SS.VIR_TOTAL_PATIENT_PRICE_BHYT \n";
                query += "FROM HIS_RS.HIS_SERE_SERV SS \n";
                query += "JOIN HIS_RS.HIS_SERVICE_REQ SR ON SS.SERVICE_REQ_ID=SR.ID  \n";
                query += "LEFT JOIN HIS_RS.HIS_SERE_SERV_BILL SSB ON SSB.SERE_SERV_ID=SS.ID  and ssb.is_cancel is null \n";
                query += "LEFT JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.ID=SSB.BILL_ID  and tran.is_cancel is null \n";
                query += "JOIN lateral (select trea.id from HIS_RS.his_treatment trea where trea.id=sr.treatment_id) TREA ON sr.TREATMENT_ID=TREA.ID  \n";
                //query += "JOIN HIS_RS.HIS_SERVICE SV ON SS.SERVICE_ID = SV.ID \n";
                //query += "LEFT JOIN HIS_RS.HIS_SERVICE PR ON SV.PARENT_ID = PR.ID \n";
                //query += "LEFT JOIN HIS_RS.V_HIS_SERVICE_RETY_CAT SRC ON (SS.SERVICE_ID=SRC.SERVICE_ID AND SRC.REPORT_TYPE_CODE='MRS00354') \n";
                //query += "JOIN HIS_RS.HIS_SERVICE_TYPE SVT ON SS.TDL_SERVICE_TYPE_ID=SVT.ID  \n";
                if (filter.INPUT_DATA_ID_TIME_TYPE == 8)
                {
                    query += "JOIN HIS_RS.HIS_HEIN_APPROVAL HAP ON SS.HEIN_APPROVAL_ID=HAP.ID  \n";
                }

                query += "WHERE 1=1 ";

                query += "AND SS.IS_NO_EXECUTE IS NULL  and sr.is_delete =0  \n";

                if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME BETWEEN {0} and {1} and tran.is_cancel is null\n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2}  and ss.is_delete =0 and ss.is_expend is null\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
                {
                    query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE ={2} and ss.is_delete =0\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    query += string.Format("AND SR.FINISH_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID ={2}  and ss.is_delete =0 and ss.is_expend is null\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    query += string.Format("AND SR.START_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID<>{2} and ss.is_delete =0\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} and {1}  and ss.is_delete =0 and ss.is_expend is null\n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
                {
                    query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1}  and ss.is_delete =0 and ss.is_expend is null\n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 8)
                {
                    query += string.Format("AND HAP.EXECUTE_TIME BETWEEN {0} and {1}  and ss.is_delete =0 and ss.is_expend is null\n", filter.TIME_FROM, filter.TIME_TO);
                }
                else
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2}  and ss.is_delete =0 and ss.is_expend is null\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);

                }
                //đối tượng thanh toán
                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID in ({0}) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
               
               
                //if (filter.TDL_PATIENT_TYPE_IDs != null)
                //{
                //    query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID in ({0}) \n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                //}


                if (filter.REQUEST_ROOM_IDs != null)
                {
                    query += string.Format("AND SR.REQUEST_ROOM_ID IN ({0}) \n", string.Join(",", filter.REQUEST_ROOM_IDs));
                }


                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }
                //if (filter.TREATMENT_TYPE_IDs != null)
                //{
                //    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID IN ({0}) \n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                //}
                //if (filter.EXACT_CASHIER_ROOM_IDs != null)
                //{
                //    query += string.Format("AND TRAN.CASHIER_ROOM_ID IN ({0}) \n", string.Join(",", filter.EXACT_CASHIER_ROOM_IDs));
                //}
                //if (filter.CASHIER_LOGINNAMEs != null)
                //{
                //    query += string.Format("AND TRAN.CASHIER_LOGINNAME IN ('{0}') \n", string.Join("','", filter.CASHIER_LOGINNAMEs));
                //}
                
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.MyAppContext().GetSql<DataGet>(query);
                if (result != null)
                {
                    result = result.GroupBy(o => o.ID).Select(p => p.First()).ToList();
                }
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
        }

    }
}

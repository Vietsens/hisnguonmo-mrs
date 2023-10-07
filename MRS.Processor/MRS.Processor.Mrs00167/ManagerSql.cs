using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MRS.Processor.Mrs00167
{
    public class ManagerSql
    {
        public List<S_HIS_SERE_SERV> GetSum(Mrs00167Filter filter)
        {
            List<S_HIS_SERE_SERV> result = null;
            try
            {
                string query = "--Thong ke benh nhan su dung dich vu\n";

                query += "select \n";
                query += "ss.IS_DELETE, \n";

                query += "ss.PATIENT_TYPE_ID, \n";

                query += "ss.IS_EXPEND, \n";

                query += "ss.PARENT_ID, \n";

                query += "ss.ID, \n";

                query += "ss.VIR_PRICE_NO_EXPEND, \n";

                query += "ss.TDL_SERVICE_NAME, \n";

                query += "ss.TDL_SERVICE_CODE, \n";

                query += "ss.TDL_REQUEST_ROOM_ID, \n";

                query += "ss.TDL_EXECUTE_ROOM_ID, \n";

                query += "ss.TDL_EXECUTE_DEPARTMENT_ID, \n";

                query += "ss.TDL_REQUEST_DEPARTMENT_ID, \n";

                query += "ss.TDL_SERVICE_TYPE_ID, \n";

                query += "ss.TDL_TREATMENT_ID, \n";

                query += "ss.AMOUNT, \n";

                query += "ss.VIR_PRICE, \n";

                query += "ss.TDL_HEIN_SERVICE_TYPE_ID, \n";

                query += "ss.SERVICE_ID, \n";

                query += "trea.TDL_PATIENT_NAME, \n";

                query += "trea.PATIENT_ID, \n";

                query += "trea.TDL_PATIENT_CODE, \n";

                query += "trea.TREATMENT_CODE, \n";

                query += "trea.IN_TIME, \n";

                query += "trea.OUT_TIME, \n";
                query += "1 \n";

                query += "from \n";
                query += "his_rs.his_sere_serv ss \n";
                query += "join his_rs.his_treatment trea on (trea.id = ss.tdl_treatment_id ) \n";
                //query += "join his_rs.his_service_req sr on sr.id = ss.service_req_id \n";
                //query += "join his_rs.his_department dp on sr.request_department_id=dp.id\n ";
                //query += "join his_rs.v_his_room room on sr.execute_room_id=room.id \n";
                //query += "LEFT JOIN HIS_RS.HIS_SERE_SERV_BILL SSB ON SSB.SERE_SERV_ID=SS.ID  \n";
                //query += "LEFT JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.ID=SSB.BILL_ID AND TRAN.IS_CANCEL IS NULL \n";
                query += "where ss.is_delete =0 and ss.is_no_execute is null \n";


                //if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                //{
                //    query += string.Format("AND TRAN.TRANSACTION_TIME BETWEEN {0} and {1} and tran.is_cancel is null\n", filter.TIME_FROM, filter.TIME_TO);
                //}
                //else if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
                //{
                //    query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2} \n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                //}
                //else if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
                //{
                query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE ={2}\n", filter.DATE_FROM, filter.DATE_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                //}
                //else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                //{
                //    query += string.Format("AND SR.FINISH_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID ={2} \n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                //}
                //else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                //{
                //    query += string.Format("AND SR.START_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID<>{2}\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL);
                //}
                //else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                //{
                //    query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                //}
                //else if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
                //{
                //    query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                //}
                //else
                //{
                //    query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2} \n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);

                //}
                //if (filter.BRANCH_ID != null)
                //{
                //    query += string.Format("and trea.branch_id = {0}", filter.BRANCH_ID);
                //}
                //if (filter.SERVICE_IDs != null)
                //{
                //    query += string.Format("and ss.SERVICE_ID in({0}) \n", string.Join(",", filter.SERVICE_IDs));
                //}
                //if (filter.END_DEPARTMENT_IDs != null)
                //{
                //    query += string.Format("and trea.END_DEPARTMENT_ID in({0}) \n", string.Join(",", filter.END_DEPARTMENT_IDs));
                //}
                //if (filter.REQUEST_DEPARTMENT_IDs != null)
                //{
                //    query += string.Format("and sr.REQUEST_DEPARTMENT_ID in({0}) \n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                //}
                //if (filter.PATIENT_TYPE_ID != null)
                //{
                //    query += string.Format("and ss.PATIENT_TYPE_ID = {0} \n", filter.PATIENT_TYPE_ID);
                //}
                //if (filter.SERVICE_REQ_STT_IDs != null)
                //{
                //    query += string.Format("and sr.service_req_stt_id in({0}) \n", string.Join(",", filter.SERVICE_REQ_STT_IDs));
                //}
                if (filter.EXECUTE_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and ss.TDL_EXECUTE_DEPARTMENT_ID in({0}) \n", string.Join(",", filter.EXECUTE_DEPARTMENT_IDs));
                }
                if (filter.REQ_ROOM_IDs != null)
                {
                    query += string.Format("and ss.TDL_REQUEST_ROOM_ID in({0}) \n", string.Join(",", filter.REQ_ROOM_IDs));
                }
                if (filter.REQ_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and ss.TDL_REQUEST_DEPARTMENT_ID in({0}) \n", string.Join(",", filter.REQ_DEPARTMENT_IDs));
                }
                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and ss.PATIENT_TYPE_ID in({0}) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (filter.SERVICE_TYPE_IDs != null)
                {
                    query += string.Format("and ss.TDL_SERVICE_TYPE_ID in({0}) \n", string.Join(",", filter.SERVICE_TYPE_IDs));
                }
                if (filter.SERVICE_ID != null)
                {
                    query += string.Format("and ss.SERVICE_ID ={0} \n",filter.SERVICE_ID);
                }
                if (filter.SERVICE_IDs != null)
                {
                    query += string.Format("and ss.service_id in({0}) \n", string.Join(",", filter.SERVICE_IDs));
                }
                //lọc theo đối tượng thanh toán
                if (filter.IS_PATIENT_TYPE.HasValue && filter.IS_PATIENT_TYPE == 1)
                {
                    query += string.Format("and ss.PATIENT_TYPE_ID <> {0} \n", HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
                }
                else if (filter.IS_PATIENT_TYPE.HasValue && filter.IS_PATIENT_TYPE == 0)
                {
                    query += string.Format("and ss.PATIENT_TYPE_ID = {0} \n", HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);//BHYT
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<S_HIS_SERE_SERV>(query);
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

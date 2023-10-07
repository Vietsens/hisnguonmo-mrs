using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00807
{
    partial class ManagerSql : BusinessBase
    {
        public List<HIS_SERE_SERV> GetSereServDO(Mrs00807Filter filter)
        {
            List<HIS_SERE_SERV> result = new List<HIS_SERE_SERV>();
            try
            {

                string query = " --chi tiet dich vu thuc hien\n";
                query += "SELECT\n";

                query += "(case when ss.tdl_service_type_id=1 then sr.execute_loginname else sr.request_loginname end) tdl_request_loginname,\n";

                query += "(case when ss.tdl_service_type_id=1 then sr.execute_username else sr.request_username end) tdl_request_username,\n";

                query += "SS.*\n";
                
                query += "FROM HIS_SERE_SERV SS\n";
                query += "join HIS_SERVICE_REQ SR on sr.id=ss.service_req_id\n";
                query += "JOIN lateral (select trea.id from his_treatment trea where trea.id=sr.treatment_id) TREA ON sr.TREATMENT_ID=TREA.ID\n";
                query += "left join his_branch br on br.id=trea.branch_id\n";
                query += "LEFT JOIN HIS_RS.HIS_SERE_SERV_BILL SSB ON SSB.SERE_SERV_ID=SS.ID and ssb.is_cancel is null \n";
                query += "LEFT JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.ID=SSB.BILL_ID AND TRAN.IS_CANCEL IS NULL \n";
                query += "WHERE SS.IS_DELETE = 0\n";
                query += "AND SR.IS_DELETE = 0\n";
                query += "AND SS.IS_NO_EXECUTE IS NULL\n";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL\n";

                //phòng thực hiện
                if (filter.EXECUTE_ROOM_ID != null)
                {
                    query += string.Format("AND SR.EXECUTE_ROOM_ID = {0}\n", filter.EXECUTE_ROOM_ID);
                }
                if (filter.EXECUTE_ROOM_IDs != null)
                {
                    query += string.Format("AND SR.EXECUTE_ROOM_ID in ({0})\n", string.Join(",", filter.EXECUTE_ROOM_IDs));
                }
                //khoa thực hiện
                if (filter.EXECUTE_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID = {0}\n", filter.EXECUTE_DEPARTMENT_ID);
                }
                if (filter.EXECUTE_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID in ({0})\n", string.Join(",", filter.EXECUTE_DEPARTMENT_IDs));
                }

                //phòng chỉ định
                if (filter.REQUEST_ROOM_ID != null)
                {
                    query += string.Format("AND SR.REQUEST_ROOM_ID = {0}\n", filter.REQUEST_ROOM_ID);
                }
                if (filter.REQUEST_ROOM_IDs != null)
                {
                    query += string.Format("AND SR.REQUEST_ROOM_ID in ({0})\n", string.Join(",", filter.REQUEST_ROOM_IDs));
                }
                //khoa chỉ định
                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID IN ({0})\n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }

                if (filter.REQUEST_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID = {0}\n", filter.REQUEST_DEPARTMENT_ID);
                }

                //đối tượng thanh toán
                if (filter.PATIENT_TYPE_ID != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID = {0}\n", filter.PATIENT_TYPE_ID);
                }

                //đối tượng bệnh nhân
                if (filter.TDL_PATIENT_TYPE_ID != null)
                {
                    query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID = {0}\n", filter.TDL_PATIENT_TYPE_ID);
                }

                if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME BETWEEN {0} and {1} and tran.is_cancel is null\n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2} \n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
                {
                    query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE ={2}\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    query += string.Format("AND SR.FINISH_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID ={2} \n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    query += string.Format("AND SR.START_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID<>{2}\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
                {
                    query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else
                {
                    query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);

                }
                //loại dịch vụ
                if (filter.SERVICE_TYPE_ID != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID = {0}\n", filter.SERVICE_TYPE_ID);
                }

                if (filter.SERVICE_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID IN ({0})\n", string.Join(",", filter.SERVICE_TYPE_IDs));
                }
                //dịch vụ
                if (filter.SERVICE_ID != null)
                {
                    query += string.Format("AND SS.SERVICE_ID = {0}\n", filter.SERVICE_ID);
                }
                if (filter.SERVICE_IDs != null)
                {
                    query += string.Format("AND SS.SERVICE_ID IN ({0})\n", string.Join(",", filter.SERVICE_IDs));
                }
                if (!string.IsNullOrEmpty(filter.SERVICE_NAME))
                {
                    query += string.Format("AND lower(SS.TDL_SERVICE_NAME) like '%{0}%'\n", filter.SERVICE_NAME.ToLower());
                }
                //nhóm dịch vụ
                if (filter.EXACT_PARENT_SERVICE_ID != null)
                {
                    query += string.Format("AND SS.SERVICE_ID IN (SELECT ID FROM HIS_SERVICE WHERE PARENT_ID= {0})\n", filter.EXACT_PARENT_SERVICE_ID);
                }
                if (filter.EXACT_PARENT_SERVICE_IDs != null)
                {
                    query += string.Format("AND SS.SERVICE_ID IN (SELECT ID FROM HIS_SERVICE WHERE PARENT_ID IN ({0}))\n", string.Join(",", filter.EXACT_PARENT_SERVICE_IDs));
                }

                //diện điều trị
                if (filter.TREATMENT_TYPE_ID != null)
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID = {0}\n", filter.TREATMENT_TYPE_ID);
                }
                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID in ({0})", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }

                //phòng thu ngân
                if (filter.EXACT_CASHIER_ROOM_ID != null)
                {
                    query += string.Format("AND TRAN.CASHIER_ROOM_ID = {0}\n", filter.EXACT_CASHIER_ROOM_ID);
                }
                if (filter.EXACT_CASHIER_ROOM_IDs != null)
                {
                    query += string.Format("AND TRAN.CASHIER_ROOM_ID in ({0})\n", string.Join(",", filter.EXACT_CASHIER_ROOM_IDs));
                }

                //nhân viên thu ngân
                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("AND TRAN.CASHIER_LOGINNAME = '{0}'\n", filter.CASHIER_LOGINNAME);
                }
                if (filter.CASHIER_LOGINNAMEs != null)
                {
                    query += string.Format("AND TRAN.CASHIER_LOGINNAME in ('{0}')\n", string.Join("','", filter.CASHIER_LOGINNAMEs));
                }


                //trạng thái y lệnh
                if (filter.SERVICE_REQ_STT_ID != null)
                {
                    query += string.Format("AND SR.SERVICE_REQ_STT_ID = {0}\n", filter.SERVICE_REQ_STT_ID);
                }
                if (filter.SERVICE_REQ_STT_IDs != null)
                {
                    query += string.Format("AND SR.SERVICE_REQ_STT_ID IN ({0})\n", string.Join(",", filter.SERVICE_REQ_STT_IDs));
                }
                //if (filter.EXECUTE_LOGINNAME_DOCTORs != null)
                //{
                //    query += string.Format("AND SR.EXECUTE_LOGINNAME IN ('{0}')\n", string.Join("','", filter.EXECUTE_LOGINNAME_DOCTORs));
                //}

                //đối tượng bệnh nhân
                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID IN ({0})\n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }

                //đối tượng thanh toán
                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID IN ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }

                //đối tượng chi tiết
                if (filter.PATIENT_CLASSIFY_ID != null)
                {
                    query += string.Format("AND SR.TDL_PATIENT_CLASSIFY_ID = {0}\n", filter.PATIENT_CLASSIFY_ID);
                }
                if (filter.PATIENT_CLASSIFY_IDs != null)
                {
                    query += string.Format("AND SR.TDL_PATIENT_CLASSIFY_ID IN ({0})\n", string.Join(",", filter.PATIENT_CLASSIFY_IDs));
                }
                if (filter.AREA_IDs != null)
                {
                    query += string.Format("AND SR.request_room_id in (select id from v_his_room where area_id IN ({0}))\n", string.Join(",", filter.AREA_IDs));
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERE_SERV>(query);

                if (rs != null)
                {
                    result = rs;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }
      
    }
}

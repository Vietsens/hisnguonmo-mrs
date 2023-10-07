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
using MRS.Processor.Mrs00166;
using System.Data;
using System.Reflection;

namespace MRS.Processor.Mrs00166
{
    public class ManagerSql
    {
        public List<Mrs00166RDO> GetSS(Mrs00166Filter filter)
        {
            const long VAO = 1;//vào viện
            const long CHIDINH = 2;//chỉ định
            const long BATDAU = 3;//bắt đầu
            const long KETTHUC = 4;//kết thúc
            const long RAVIEN = 5;//ra viện
            const long THANHTOAN = 6;//thanh toán
            const long KHOAVIENPHI = 7;//khóa viện phí
            const long GIAMDINHBHYT = 8;//giám định bhyt
            List<Mrs00166RDO> result = null;
            try
            {
                string query = "";
                query += string.Format("--danh sach dich vu chi tiet\n");
                query += string.Format("select\n");
                query += string.Format("trea.*,\n");
                query += string.Format("ss.tdl_service_code SERVICE_CODE,\n");
                query += string.Format("ss.tdl_service_name SERVICE_NAME,\n");
                query += string.Format("ss.TDL_HEIN_SERVICE_BHYT_CODE,\n");
                query += string.Format("ss.TDL_HEIN_SERVICE_BHYT_NAME,\n");
                query += string.Format("ss.AMOUNT,\n");
                query += string.Format("ss.SERVICE_ID,\n");
                query += string.Format("ss.TDL_SERVICE_TYPE_ID,\n");
                query += string.Format("ss.TDL_HEIN_SERVICE_TYPE_ID,\n");
                query += string.Format("ss.PATIENT_TYPE_ID,\n");
                query += string.Format("sr.TREATMENT_ID,\n");
                query += string.Format("nvl(ss.vir_price,0) PRICE,\n");
                query += string.Format("nvl(ss.vir_total_price,0) TOTAL_PRICE,\n");
                //query += string.Format("trea.LAST_DEPARTMENT_ID,\n");//
                //query += string.Format("trea.IN_TIME,\n");//
                //query += string.Format("trea.OUT_TIME,\n");//
                //query += string.Format("trea.TDL_PATIENT_DOB,\n");//
                query += string.Format("ss.TDL_INTRUCTION_TIME,\n");
                query += string.Format("sr.REQUEST_ROOM_ID,\n");//
                query += string.Format("sr.REQUEST_USERNAME,\n");
                query += string.Format("sr.EXECUTE_ROOM_ID,\n");//
                query += string.Format("sr.EXECUTE_USERNAME,\n");
                //query += string.Format("ss.DIPLOMA,\n");
                //query += string.Format("trea.TDL_TREATMENT_TYPE_ID,\n");//
                query += string.Format("sr.REQUEST_DEPARTMENT_ID, \n");//

                //query += string.Format("ss.REQUEST_DEPARTMENT_CODE,\n");
                query += string.Format("sr.EXECUTE_DEPARTMENT_ID,\n");//

                query += string.Format("sr.FINISH_TIME TDL_FINISH_TIME,\n");

                query += string.Format("sr.REQUEST_ROOM_ID,\n");//

                //query += string.Format("ss.EXECUTE_ROOM_CODE,\n");

                query += string.Format("sr.REQUEST_LOGINNAME,\n");

                query += string.Format("sr.EXECUTE_LOGINNAME,\n");

                // query += string.Format("ss.TREATMENT_TYPE_CODE,\n");

                //query += string.Format("ss.EXE_DIPLOMA,\n");

                query += string.Format("sr.START_TIME EXECUTE_TIME,\n");

                query += string.Format("ss.OTHER_PAY_SOURCE_ID SS_OTHER_PAY_SOURCE_ID,\n");

                query += string.Format("sse.BEGIN_TIME,\n");

                query += string.Format("sse.END_TIME,\n");

                query += string.Format("1\n");

                query += string.Format("from his_sere_serv ss\n");
                query += string.Format("join his_treatment trea on trea.id=ss.tdl_treatment_id\n");
                //query += string.Format("left join his_sere_serv_bill ssb on ssb.sere_serv_id=SS.ID\n");
                //query += string.Format("left join his_transaction tran on tran.id=ssb.bill_id\n");
                //query += string.Format("left join his_hein_approval hap on hap.id=ss.hein_approval_id\n");
                query += string.Format("join his_service_req sr on sr.id=ss.service_req_id\n");
                query += string.Format("left join his_sere_serv_ext sse on sse.sere_serv_id=ss.id\n");
                query += string.Format("where 1=1\n");
                query += string.Format("and ss.is_delete=0\n");
                query += string.Format("and ss.is_no_execute is null\n");

                if (filter.INPUT_DATA_ID_TIME_TYPE == GIAMDINHBHYT)
                {
                    query += string.Format("AND ss.hein_approval_id in (select id from his_hein_approva hap where hap.EXECUTE_TIME BETWEEN {0} and {1})\n", filter.TIME_FROM??filter.IN_TIME_FROM??filter.OUT_TIME_FROM??filter.FEE_LOCK_TIME_FROM??filter.HEIN_LOCK_TIME_FROM, filter.TIME_TO??filter.IN_TIME_TO??filter.OUT_TIME_TO??filter.FEE_LOCK_TIME_TO??filter.HEIN_LOCK_TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == THANHTOAN)
                {
                    query += string.Format("AND ss.id in (select sere_serv_id from his_sere_serv_bill where bill_id in (select id from his_transaction tran where TRAN.TRANSACTION_TIME BETWEEN {0} and {1} and tran.is_cancel is null))\n", filter.TIME_FROM??filter.IN_TIME_FROM??filter.OUT_TIME_FROM??filter.FEE_LOCK_TIME_FROM??filter.HEIN_LOCK_TIME_FROM, filter.TIME_TO??filter.IN_TIME_TO??filter.OUT_TIME_TO??filter.FEE_LOCK_TIME_TO??filter.HEIN_LOCK_TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == KHOAVIENPHI)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2} \n", filter.TIME_FROM??filter.IN_TIME_FROM??filter.OUT_TIME_FROM??filter.FEE_LOCK_TIME_FROM??filter.HEIN_LOCK_TIME_FROM, filter.TIME_TO??filter.IN_TIME_TO??filter.OUT_TIME_TO??filter.FEE_LOCK_TIME_TO??filter.HEIN_LOCK_TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == RAVIEN)
                {
                    query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE ={2}\n", filter.TIME_FROM??filter.IN_TIME_FROM??filter.OUT_TIME_FROM??filter.FEE_LOCK_TIME_FROM??filter.HEIN_LOCK_TIME_FROM, filter.TIME_TO??filter.IN_TIME_TO??filter.OUT_TIME_TO??filter.FEE_LOCK_TIME_TO??filter.HEIN_LOCK_TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == KETTHUC)
                {
                    query += string.Format("AND SR.FINISH_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID ={2} \n", filter.TIME_FROM??filter.IN_TIME_FROM??filter.OUT_TIME_FROM??filter.FEE_LOCK_TIME_FROM??filter.HEIN_LOCK_TIME_FROM, filter.TIME_TO??filter.IN_TIME_TO??filter.OUT_TIME_TO??filter.FEE_LOCK_TIME_TO??filter.HEIN_LOCK_TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == BATDAU)
                {
                    query += string.Format("AND SR.START_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID<>{2}\n", filter.TIME_FROM??filter.IN_TIME_FROM??filter.OUT_TIME_FROM??filter.FEE_LOCK_TIME_FROM??filter.HEIN_LOCK_TIME_FROM, filter.TIME_TO??filter.IN_TIME_TO??filter.OUT_TIME_TO??filter.FEE_LOCK_TIME_TO??filter.HEIN_LOCK_TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == CHIDINH)
                {
                    query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM??filter.IN_TIME_FROM??filter.OUT_TIME_FROM??filter.FEE_LOCK_TIME_FROM??filter.HEIN_LOCK_TIME_FROM, filter.TIME_TO??filter.IN_TIME_TO??filter.OUT_TIME_TO??filter.FEE_LOCK_TIME_TO??filter.HEIN_LOCK_TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == VAO)
                {
                    query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM??filter.IN_TIME_FROM??filter.OUT_TIME_FROM??filter.FEE_LOCK_TIME_FROM??filter.HEIN_LOCK_TIME_FROM, filter.TIME_TO??filter.IN_TIME_TO??filter.OUT_TIME_TO??filter.FEE_LOCK_TIME_TO??filter.HEIN_LOCK_TIME_TO);
                }
                else
                {
                    query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE ={2} \n", filter.TIME_FROM ?? filter.IN_TIME_FROM ?? filter.OUT_TIME_FROM ?? filter.FEE_LOCK_TIME_FROM ?? filter.HEIN_LOCK_TIME_FROM, filter.TIME_TO ?? filter.IN_TIME_TO ?? filter.OUT_TIME_TO ?? filter.FEE_LOCK_TIME_TO ?? filter.HEIN_LOCK_TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);

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

                //trạng thái y lệnh
                if (filter.SERVICE_REQ_STT_ID != null)
                {
                    query += string.Format("AND SR.SERVICE_REQ_STT_ID = {0}\n", filter.SERVICE_REQ_STT_ID);
                }
                if (filter.SERVICE_REQ_STT_IDs != null)
                {
                    query += string.Format("AND SR.SERVICE_REQ_STT_ID IN ({0})\n", string.Join(",", filter.SERVICE_REQ_STT_IDs));
                }

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

                //trạng khoa chỉ định
                if (filter.REQUEST_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID = {0}\n", filter.REQUEST_DEPARTMENT_ID);
                }
                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID IN ({0})\n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }

                //khoa thực hiện
                if (filter.EXECUTE_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID = {0}\n", filter.EXECUTE_DEPARTMENT_ID);
                }
                if (filter.EXECUTE_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID IN ({0})\n", string.Join(",", filter.EXECUTE_DEPARTMENT_IDs));
                }

                //khoa ra viện
                if (filter.END_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND trea.LAST_DEPARTMENT_ID = {0}\n", filter.END_DEPARTMENT_ID);
                }
                if (filter.END_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND trea.LAST_DEPARTMENT_ID IN ({0})\n", string.Join(",", filter.END_DEPARTMENT_IDs));
                }
                //khoa, loại khoa
                if (filter.DEPARTMENT_ID != null)
                {
                    if (filter.CHOOSE_DEPARTMENT == null)
                    {
                        query += string.Format("AND trea.LAST_DEPARTMENT_ID = {0}\n", filter.DEPARTMENT_ID);

                    }
                    else if (filter.CHOOSE_DEPARTMENT == false)
                    {
                        query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID = {0}\n", filter.DEPARTMENT_ID);
                    }
                    else if (filter.CHOOSE_DEPARTMENT == true)
                    {
                        query += string.Format("AND SR.REQUEST_DEPARTMENT_ID = {0}\n", filter.DEPARTMENT_ID);
                    }
                }
                if (filter.DEPARTMENT_IDs != null)
                {
                    if (filter.CHOOSE_DEPARTMENT == null)
                    {
                        query += string.Format("AND trea.LAST_DEPARTMENT_ID IN ({0})\n", string.Join(",", filter.DEPARTMENT_IDs));

                    }
                    else if (filter.CHOOSE_DEPARTMENT == false)
                    {
                        query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID IN ({0})\n", string.Join(",", filter.DEPARTMENT_IDs));
                    }
                    else if (filter.CHOOSE_DEPARTMENT == true)
                    {
                        query += string.Format("AND SR.REQUEST_DEPARTMENT_ID IN ({0})\n", string.Join(",", filter.DEPARTMENT_IDs));
                    }
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00166RDO>(query);
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

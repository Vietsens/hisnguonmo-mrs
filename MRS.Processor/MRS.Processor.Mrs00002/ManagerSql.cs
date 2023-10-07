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
using System.Reflection;

namespace MRS.Processor.Mrs00002
{
    public  class ManagerSql
    {
        public List<TREATMENT> GetTreatment(Mrs00002Filter filter)
        {
            List<TREATMENT> result = null;
            try
            {
                string query = "";
                query += string.Format("--du lieu ho so dieu tri co y lenh kham\n");
                query += string.Format("select \n");
                query += string.Format("trea.id,\n");
                query += string.Format("trea.patient_id,\n");
                query += string.Format("trea.tdl_patient_classify_id,\n");
                query += string.Format("trea.APPOINTMENT_TIME,\n");
                query += string.Format("trea.TDL_HEIN_CARD_NUMBER,\n");
                query += string.Format("trea.tdl_treatment_type_id,\n");
                query += string.Format("trea.in_room_id,\n");
                query += string.Format("trea.in_treatment_type_id,\n");
                query += string.Format("trea.end_department_id,\n");
                query += string.Format("trea.out_date,\n");
                query += string.Format("trea.is_pause,\n");
                query += string.Format("trea.treatment_end_type_id,\n");
                query += string.Format("trea.out_time,\n");
                query += string.Format("trea.clinical_in_time,\n");
                query += string.Format("trea.end_room_id,\n");
                query += string.Format("trea.TDL_KSK_CONTRACT_ID,\n");
                query += string.Format("trea.TDL_PATIENT_NATIONAL_NAME,\n");
                query += string.Format(" (case when trea.tdl_patient_province_code = br.province_code then 1 else null end) IS_IN_PROVINCE,\n");
                query += string.Format("trea.tran_pati_form_id\n");
                //query += string.Format("trea.IS_EMERGENCY,\n");
                //query += string.Format("CASE WHEN trea.IS_EMERGENCY = 1 THEN 'x' WHEN trea.IS_EMERGENCY is null THEN '  ' END AS CAP_CUU,\n");

                query += string.Format("from his_treatment trea\n");
                query += string.Format("left join his_branch br on br.id=trea.branch_id\n");
                query += string.Format("join \n");
                query += string.Format("( \n");
                query += string.Format("select \n");
                query += string.Format("sr.treatment_id\n");
                query += string.Format("from his_service_req sr\n");
                query += string.Format("join v_his_room ro on ro.id=sr.request_room_id\n");
                
                query += string.Format("where 1=1\n");
                if (filter.EXAM_ROOM_IDs != null)
                {
                    query += string.Format("and sr.execute_room_id in ({0})\n", string.Join(",", filter.EXAM_ROOM_IDs));
                }
                if (filter.DEPARTMENT_IDs != null)
                {
                    query += string.Format("and sr.execute_department_id in ({0})\n", string.Join(",", filter.DEPARTMENT_IDs));
                }
                query += string.Format("and (ro.room_type_id ={0} or ro.is_exam ={1})\n",IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD,IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);

                if (filter.CREATE_TIME_FROM > 0)
                {
                    query += string.Format("and sr.create_time >= {0}\n", filter.CREATE_TIME_FROM);
                }
                if (filter.CREATE_TIME_TO > 0)
                {
                    query += string.Format("and sr.create_time <= {0}\n", filter.CREATE_TIME_TO);
                }

                if (filter.IS_BILL_TIME == true)
                {
                    if (filter.INTRUCTION_TIME_FROM > 0 && filter.INTRUCTION_TIME_TO > 0)
                    {
                        query += string.Format("and exists(select 1 from his_sere_serv_bill ssb where ssb.tdl_service_req_id=sr.id and ssb.is_cancel is null and ssb.create_time between {0} and {1} or select 1 from his_sere_serv_deposit ssd where ssd.tdl_service_req_id=sr.id and ssd.is_cancel is null and ssd.create_time between {0} and {1})\n", filter.INTRUCTION_TIME_FROM, filter.INTRUCTION_TIME_TO);
                    }
                    
                }
                else
                {
                    if (filter.IS_INTRUCTION_TIME_OR_FINISH_TIME == false)
                    {
                        if (filter.INTRUCTION_TIME_FROM > 0)
                        {
                            query += string.Format("and sr.finish_time >= {0}\n", filter.INTRUCTION_TIME_FROM);
                        }
                        if (filter.INTRUCTION_TIME_TO > 0)
                        {
                            query += string.Format("and sr.finish_time <= {0}\n", filter.INTRUCTION_TIME_TO);
                        }
                    }
                    else
                    {
                        if (filter.INTRUCTION_TIME_FROM > 0)
                        {
                            query += string.Format("and sr.intruction_time >= {0}\n", filter.INTRUCTION_TIME_FROM);
                        }
                        if (filter.INTRUCTION_TIME_TO > 0)
                        {
                            query += string.Format("and sr.intruction_time <= {0}\n", filter.INTRUCTION_TIME_TO);
                        }
                    }
                }
                query += string.Format("and sr.is_no_execute is null\n");
                query += string.Format("and sr.is_delete=0\n");
                query += string.Format("and sr.service_req_type_id=1\n");
                query += string.Format("group by\n");
                query += string.Format("sr.treatment_id\n");
                query += string.Format(") sr on sr.treatment_id=trea.id \n");

                query += string.Format("where 1=1\n");

                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.tdl_treatment_type_id in ({0})\n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }

                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }
                if (filter.BRANCH_IDs != null)
                {
                    query += string.Format("and trea.branch_id in ({0})\n", string.Join(",", filter.BRANCH_IDs));
                }

                if (filter.INPUT_DATA_ID_STT_TYPE == 1)
                {
                    query += string.Format("and trea.is_pause = 1\n");
                }
                if (filter.INPUT_DATA_ID_STT_TYPE == 2)
                {
                    query += string.Format("and trea.is_pause is null\n");
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<TREATMENT>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<PATIENT_TYPE_ALTER> GetPatientTypeAlter(Mrs00002Filter filter)
        {
            List<PATIENT_TYPE_ALTER> result = null;
            try
            {
                string query = "";
                query += string.Format("--du lieu chuyen doi tuong cua cac ho so dieu tri co y lenh kham\n");
                query += string.Format("select \n");
                query += "pta.treatment_id, \n";
                query += "pta.log_time, \n";
                query += "pta.right_route_code, \n";
                query += "pta.patient_type_id \n";
                query += string.Format("from his_patient_type_alter pta\n");
                query += string.Format("join \n");
                query += string.Format("( \n");
                query += string.Format("select \n");
                query += string.Format("sr.treatment_id\n");
                query += string.Format("from his_service_req sr\n");
                query += string.Format("join v_his_room ro on ro.id=sr.request_room_id\n");
               
                query += string.Format("where 1=1\n");
                if (filter.EXAM_ROOM_IDs != null)
                {
                    query += string.Format("and sr.execute_room_id in ({0})\n", string.Join(",", filter.EXAM_ROOM_IDs));
                }
                if (filter.DEPARTMENT_IDs != null)
                {
                    query += string.Format("and sr.execute_department_id in ({0})\n", string.Join(",", filter.DEPARTMENT_IDs));
                }
                query += string.Format("and (ro.room_type_id ={0} or ro.is_exam ={1})\n", IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                if (filter.CREATE_TIME_TO > 0)
                {
                    query += string.Format("and sr.create_time <= {0}\n", filter.CREATE_TIME_TO);
                }


                if (filter.IS_BILL_TIME == true)
                {
                    if (filter.INTRUCTION_TIME_FROM > 0 && filter.INTRUCTION_TIME_TO > 0)
                    {
                        query += string.Format("and exists(select 1 from his_sere_serv_bill ssb where ssb.tdl_service_req_id=sr.id and ssb.is_cancel is null and ssb.create_time between {0} and {1} or select 1 from his_sere_serv_deposit ssd where ssd.tdl_service_req_id=sr.id and ssd.is_cancel is null and ssd.create_time between {0} and {1})\n", filter.INTRUCTION_TIME_FROM, filter.INTRUCTION_TIME_TO);
                    }

                }
                else
                {
                    if (filter.IS_INTRUCTION_TIME_OR_FINISH_TIME == false)
                    {
                        if (filter.INTRUCTION_TIME_FROM > 0)
                        {
                            query += string.Format("and sr.finish_time >= {0}\n", filter.INTRUCTION_TIME_FROM);
                        }
                        if (filter.INTRUCTION_TIME_TO > 0)
                        {
                            query += string.Format("and sr.finish_time <= {0}\n", filter.INTRUCTION_TIME_TO);
                        }
                    }
                    else
                    {
                        if (filter.INTRUCTION_TIME_FROM > 0)
                        {
                            query += string.Format("and sr.intruction_time >= {0}\n", filter.INTRUCTION_TIME_FROM);
                        }
                        if (filter.INTRUCTION_TIME_TO > 0)
                        {
                            query += string.Format("and sr.intruction_time <= {0}\n", filter.INTRUCTION_TIME_TO);
                        }
                    }
                }
                query += string.Format("and sr.is_no_execute is null\n");
                query += string.Format("and sr.is_delete=0\n");
                query += string.Format("and sr.service_req_type_id=1\n");
                query += string.Format("group by\n");
                query += string.Format("sr.treatment_id\n");
                query += string.Format(") sr on sr.treatment_id=pta.treatment_id \n");
                
                query += string.Format("where 1=1\n");
               
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<PATIENT_TYPE_ALTER>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
        public List<SERVICE_REQ> GetServiceReq(Mrs00002Filter filter)
        {
            List<SERVICE_REQ> result = null;
            try
            {
                string query = "";
                query += string.Format("--du lieu y lenh kham\n");
                query += string.Format("select \n");
                query += string.Format("sr.id,\n");
                query += string.Format("sr.tdl_patient_classify_id,\n");
                query += string.Format("sr.execute_room_id,\n");
                query += string.Format("sr.request_room_id,\n");
                query += string.Format("sr.treatment_id,\n");
                query += string.Format("sr.intruction_time,\n");
                query += string.Format("sr.intruction_date,\n");
                query += string.Format("sr.previous_service_req_id,\n");
                query += string.Format("sr.tdl_patient_gender_id,\n");
                query += string.Format("sr.is_main_exam,\n");
                query += string.Format("sr.is_emergency,\n");
                query += string.Format("sr.tdl_patient_dob,\n");
                query += string.Format("sr.TDL_PATIENT_ADDRESS,\n");
                query += string.Format("sr.service_req_stt_id,\n");
                query += string.Format("ss.hein_ratio,\n");
                query += string.Format("sr.finish_time\n");
                query += string.Format("from his_service_req sr\n");
                query += string.Format("left join his_sere_serv ss on sr.id = ss.service_req_id\n");
                query += string.Format("left join his_treatment trea on trea.id = sr.treatment_id\n");
               
                query += string.Format("join v_his_room ro on ro.id=sr.request_room_id\n");
                query += string.Format("where 1=1\n");
                if (filter.EXAM_ROOM_IDs != null)
                {
                    query += string.Format("and sr.execute_room_id in ({0})\n", string.Join(",", filter.EXAM_ROOM_IDs));
                }
                if (filter.DEPARTMENT_IDs != null)
                {
                    query += string.Format("and sr.execute_department_id in ({0})\n", string.Join(",", filter.DEPARTMENT_IDs));
                }
                query += string.Format("and (ro.room_type_id ={0} or ro.is_exam ={1})\n", IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                if (filter.CREATE_TIME_FROM > 0)
                {
                    query += string.Format("and sr.create_time >= {0}\n", filter.CREATE_TIME_FROM);
                }
                if (filter.CREATE_TIME_TO > 0)
                {
                    query += string.Format("and sr.create_time <= {0}\n", filter.CREATE_TIME_TO);
                }

                if (filter.IS_BILL_TIME == true)
                {
                    if (filter.INTRUCTION_TIME_FROM > 0 && filter.INTRUCTION_TIME_TO > 0)
                    {
                        query += string.Format("and exists(select 1 from his_sere_serv_bill ssb where ssb.tdl_service_req_id=sr.id and ssb.is_cancel is null and ssb.create_time between {0} and {1} or select 1 from his_sere_serv_deposit ssd where ssd.tdl_service_req_id=sr.id and ssd.is_cancel is null and ssd.create_time between {0} and {1})\n", filter.INTRUCTION_TIME_FROM, filter.INTRUCTION_TIME_TO);
                    }

                }
                else
                {
                    if (filter.IS_INTRUCTION_TIME_OR_FINISH_TIME == false)
                    {
                        if (filter.INTRUCTION_TIME_FROM > 0)
                        {
                            query += string.Format("and sr.finish_time >= {0}\n", filter.INTRUCTION_TIME_FROM);
                        }
                        if (filter.INTRUCTION_TIME_TO > 0)
                        {
                            query += string.Format("and sr.finish_time <= {0}\n", filter.INTRUCTION_TIME_TO);
                        }
                    }
                    else
                    {
                        if (filter.INTRUCTION_TIME_FROM > 0)
                        {
                            query += string.Format("and sr.intruction_time >= {0}\n", filter.INTRUCTION_TIME_FROM);
                        }
                        if (filter.INTRUCTION_TIME_TO > 0)
                        {
                            query += string.Format("and sr.intruction_time <= {0}\n", filter.INTRUCTION_TIME_TO);
                        }
                    }
                }
                if (filter.INPUT_DATA_ID_STT_TYPE == 1)
                {
                    query += string.Format("and trea.is_pause = 1\n");
                }
                if (filter.INPUT_DATA_ID_STT_TYPE == 2)
                {
                    query += string.Format("and trea.is_pause is null\n");
                }
                query += string.Format("and sr.is_no_execute is null\n");
                query += string.Format("and sr.is_delete=0\n");
                query += string.Format("and sr.service_req_type_id=1\n");
                query += string.Format("order by sr.id\n");
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<SERVICE_REQ>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<TREATMENT> GetByTreatmentOutOfKCC_KKB(Mrs00002Filter filter, List<long> KccDepartmentIds)
        {
            List<TREATMENT> result = null;
            try
            {
                if (KccDepartmentIds == null || KccDepartmentIds.Count == 0)
                {
                    return null;
                }
                string query = "--du lieu ho so dieu tri vao noi tru va ra khoi khoa kham benh\r\n";
                query += "SELECT \n";
                query += string.Format("trea.id,\n");
                query += string.Format("trea.TDL_HEIN_CARD_NUMBER,\n");
                query += string.Format("trea.tdl_treatment_type_id,\n");
                query += string.Format("trea.in_room_id,\n");
                query += string.Format("trea.in_treatment_type_id,\n");
                query += string.Format("trea.end_department_id,\n");
                query += string.Format("trea.out_date,\n");
                query += string.Format("trea.is_pause,\n");
                query += string.Format("trea.treatment_end_type_id,\n");
                query += string.Format("trea.out_time,\n");
                query += string.Format("trea.clinical_in_time,\n");
                query += string.Format("trea.end_room_id,\n");
                query += string.Format("trea.tran_pati_form_id\n");

                query += "from his_treatment trea \n";
                query += string.Format("join his_patient_type_alter pta on pta.treatment_id=trea.id\n");
                query += string.Format("join his_department_tran dpt on dpt.id=pta.department_tran_id \n");
                query += string.Format("left join his_department_tran next on next.previous_id=dpt.id \n");
                query += string.Format("where 1=1\n");
                query += string.Format("and pta.treatment_type_id in ({0})\n", string.Join(",", new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU }));
                query += string.Format("and dpt.department_id in ({0})\n", string.Join(",", KccDepartmentIds));
                query += string.Format("and (case when next.department_in_time>0 then next.department_in_time when trea.is_pause = {2} and trea.end_department_id=dpt.department_id then trea.out_time else {0}-1 end) between {0} and {1}\n", filter.INTRUCTION_TIME_FROM ?? filter.CREATE_TIME_FROM, filter.INTRUCTION_TIME_TO ?? filter.CREATE_TIME_TO,IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);

                if (filter.DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND trea.in_department_id in({0}) \n", string.Join(",", filter.DEPARTMENT_IDs));
                }

                if (filter.EXAM_ROOM_IDs != null)
                {
                    query += string.Format("AND trea.in_room_id in({0}) \n", string.Join(",", filter.EXAM_ROOM_IDs));
                }


                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }
                if (filter.BRANCH_IDs != null)
                {
                    query += string.Format("and trea.branch_id in ({0})\n", string.Join(",", filter.BRANCH_IDs));
                }

                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.tdl_treatment_type_id in ({0})\n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }
                if (filter.INPUT_DATA_ID_STT_TYPE == 1)
                {
                    query += string.Format("and trea.is_pause = 1\n");
                }
                if (filter.INPUT_DATA_ID_STT_TYPE == 2)
                {
                    query += string.Format("and trea.is_pause is null\n");
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<TREATMENT>(query);
                result = result.GroupBy(p => p.ID).Select(o => o.First()).ToList();

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<SERVICE_REQ> GetServiceReqOutOfKCC_KKB(Mrs00002Filter filter, List<long> KccDepartmentIds)
        {
            List<SERVICE_REQ> result = null;
            try
            {
                if (KccDepartmentIds == null || KccDepartmentIds.Count == 0)
                {
                    return null;
                }
                string query = "--du lieu y lenh kham vao noi tru va ra khoi khoa kham benh cap cuu\r\n";
                query += "SELECT \n";
                query += string.Format("sr.id,\n");
                query += string.Format("sr.tdl_patient_classify_id,\n");
                query += string.Format("sr.execute_room_id,\n");
                query += string.Format("sr.request_room_id,\n");
                query += string.Format("sr.treatment_id,\n");
                query += string.Format("sr.intruction_time,\n");
                query += string.Format("sr.previous_service_req_id,\n");
                query += string.Format("sr.tdl_patient_gender_id,\n");
                query += string.Format("sr.is_emergency,\n");
                query += string.Format("sr.tdl_patient_dob,\n");
                query += string.Format("sr.service_req_stt_id,\n");
                query += string.Format("ss.hein_ratio,\n");
                query += string.Format("sr.finish_time\n");
                query += string.Format("from his_service_req sr\n");
                query += string.Format("join his_sere_serv ss on sr.id = ss.service_req_id\n");
                query += string.Format("join v_his_room ro on ro.id=sr.request_room_id\n");
                query += string.Format("join (\n");
                query += string.Format("select \n");
                query += string.Format("pta.treatment_id\n");
                query += "from his_treatment trea \n";
                query += string.Format("join his_patient_type_alter pta on pta.treatment_id=trea.id\n");
                query += string.Format("join his_department_tran dpt on dpt.id=pta.department_tran_id \n");
                query += string.Format("left join his_department_tran next on next.previous_id=dpt.id \n");
                query += string.Format("where 1=1\n");
                query += string.Format("and pta.treatment_type_id in ({0})\n", string.Join(",", new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU }));
                query += string.Format("and dpt.department_id in ({0})\n", string.Join(",", KccDepartmentIds));
                query += string.Format("and (case when next.department_in_time>0 then next.department_in_time when trea.is_pause = {2} and trea.end_department_id=dpt.department_id then trea.out_time else {0}-1 end) between {0} and {1}\n", filter.INTRUCTION_TIME_FROM ?? filter.CREATE_TIME_FROM, filter.INTRUCTION_TIME_TO ?? filter.CREATE_TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);

                if (filter.DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND trea.in_department_id in({0}) \n", string.Join(",", filter.DEPARTMENT_IDs));
                }

                if (filter.EXAM_ROOM_IDs != null)
                {
                    query += string.Format("AND trea.in_room_id in({0}) \n", string.Join(",", filter.EXAM_ROOM_IDs));
                }
                query += string.Format("group by \n");
                query += string.Format("pta.treatment_id\n");
                query += string.Format(") pta on pta.treatment_id=sr.treatment_id \n");

                query += "where 1 = 1 \n";
              
                query += string.Format("and sr.is_no_execute is null\n");
                query += string.Format("and sr.is_delete=0\n");
                query += string.Format("and sr.service_req_type_id=1\n");
                if (filter.EXAM_ROOM_IDs != null)
                {
                    query += string.Format("and sr.execute_room_id in ({0})\n", string.Join(",", filter.EXAM_ROOM_IDs));
                }
                if (filter.DEPARTMENT_IDs != null)
                {
                    query += string.Format("and sr.execute_department_id in ({0})\n", string.Join(",", filter.DEPARTMENT_IDs));
                }
                query += string.Format("and (ro.room_type_id ={0} or ro.is_exam ={1})\n", IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<SERVICE_REQ>(query);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<PATIENT_TYPE_ALTER> GetPtaOfKCC_KKB(Mrs00002Filter filter, List<long> KccDepartmentIds)
        {
            List<PATIENT_TYPE_ALTER> result = null;
            try
            {
                if (KccDepartmentIds == null || KccDepartmentIds.Count == 0)
                {
                    return null;
                }
                string query = "--du lieu chuyen doi tuong cua ho so dieu tri vao noi tru va ra khoi khoa kham benh cap cuu\r\n";
                query += "SELECT \n";
                query += "pta.treatment_id, \n";
                query += "pta.log_time, \n";
                query += "pta.right_route_code, \n";
                query += "pta.patient_type_id \n";
                query += string.Format("from his_patient_type_alter pta\n");
                query += string.Format("join (\n");
                query += string.Format("select \n");
                query += string.Format("pta.treatment_id\n");
                query += "from his_treatment trea \n";
                query += string.Format("join his_patient_type_alter pta on pta.treatment_id=trea.id\n");
                query += string.Format("join his_department_tran dpt on dpt.id=pta.department_tran_id \n");
                query += string.Format("left join his_department_tran next on next.previous_id=dpt.id \n");
                query += string.Format("where 1=1\n");
                query += string.Format("and pta.treatment_type_id in ({0})\n", string.Join(",", new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU }));
                query += string.Format("and dpt.department_id in ({0})\n", string.Join(",", KccDepartmentIds));
                query += string.Format("and (case when next.department_in_time>0 then next.department_in_time when trea.is_pause = {2} and trea.end_department_id=dpt.department_id then trea.out_time else {0}-1 end) between {0} and {1}\n", filter.INTRUCTION_TIME_FROM ?? filter.CREATE_TIME_FROM, filter.INTRUCTION_TIME_TO ?? filter.CREATE_TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);

                if (filter.DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND trea.in_department_id in({0}) \n", string.Join(",", filter.DEPARTMENT_IDs));
                }

                if (filter.EXAM_ROOM_IDs != null)
                {
                    query += string.Format("AND trea.in_room_id in({0}) \n", string.Join(",", filter.EXAM_ROOM_IDs));
                }
                query += string.Format("group by \n");
                query += string.Format("pta.treatment_id\n");
                query += string.Format(") pta1 on pta1.treatment_id=pta.treatment_id \n");

                query += "where 1 = 1 \n";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<PATIENT_TYPE_ALTER>(query);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<TREATMENT> GetByIdRemove(Mrs00002Filter filter, List<long> KccDepartmentIds)
        {
            List<TREATMENT> result = null;
            try
            {
                if (KccDepartmentIds == null || KccDepartmentIds.Count == 0)
                {
                    return null;
                }
                string query = "--cac ho so dieu tri co kham va vao noi tru tai khoa kham benh cap cuu\n";
                query += "SELECT \n";
                query += "trea.id \n";
                query += string.Format("from his_treatment trea\n");
                query += string.Format("join \n");
                query += string.Format("( \n");
                query += string.Format("select \n");
                query += string.Format("sr.treatment_id\n");
                query += string.Format("from his_service_req sr\n");
               
                query += string.Format("join v_his_room ro on ro.id=sr.request_room_id\n");
                query += string.Format("where 1=1\n");
                if (filter.CREATE_TIME_FROM > 0)
                {
                    query += string.Format("and sr.create_time >= {0}\n", filter.CREATE_TIME_FROM);
                }
                if (filter.CREATE_TIME_TO > 0)
                {
                    query += string.Format("and sr.create_time <= {0}\n", filter.CREATE_TIME_TO);
                }
                if (filter.EXAM_ROOM_IDs != null)
                {
                    query += string.Format("and sr.execute_room_id in ({0})\n", string.Join(",", filter.EXAM_ROOM_IDs));
                }
                if (filter.DEPARTMENT_IDs != null)
                {
                    query += string.Format("and sr.execute_department_id in ({0})\n", string.Join(",", filter.DEPARTMENT_IDs));
                }
                query += string.Format("and (ro.room_type_id ={0} or ro.is_exam ={1})\n", IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);


                if (filter.IS_BILL_TIME == true)
                {
                    if (filter.INTRUCTION_TIME_FROM > 0 && filter.INTRUCTION_TIME_TO > 0)
                    {
                        query += string.Format("and exists(select 1 from his_sere_serv_bill ssb where ssb.tdl_service_req_id=sr.id and ssb.is_cancel is null and ssb.create_time between {0} and {1} or select 1 from his_sere_serv_deposit ssd where ssd.tdl_service_req_id=sr.id and ssd.is_cancel is null and ssd.create_time between {0} and {1})\n", filter.INTRUCTION_TIME_FROM, filter.INTRUCTION_TIME_TO);
                    }

                }
                else
                {
                    if (filter.IS_INTRUCTION_TIME_OR_FINISH_TIME == false)
                    {
                        if (filter.INTRUCTION_TIME_FROM > 0)
                        {
                            query += string.Format("and sr.finish_time >= {0}\n", filter.INTRUCTION_TIME_FROM);
                        }
                        if (filter.INTRUCTION_TIME_TO > 0)
                        {
                            query += string.Format("and sr.finish_time <= {0}\n", filter.INTRUCTION_TIME_TO);
                        }
                    }
                    else
                    {
                        if (filter.INTRUCTION_TIME_FROM > 0)
                        {
                            query += string.Format("and sr.intruction_time >= {0}\n", filter.INTRUCTION_TIME_FROM);
                        }
                        if (filter.INTRUCTION_TIME_TO > 0)
                        {
                            query += string.Format("and sr.intruction_time <= {0}\n", filter.INTRUCTION_TIME_TO);
                        }
                    }
                }
                query += string.Format("and sr.is_no_execute is null\n");
                query += string.Format("and sr.is_delete=0\n");
                query += string.Format("and sr.service_req_type_id=1\n");
                query += string.Format("group by\n");
                query += string.Format("sr.treatment_id\n");
                query += string.Format(") sr on sr.treatment_id=trea.id \n");

                query += string.Format("join (\n");
                query += string.Format("select \n");
                query += string.Format("pta.treatment_id\n");
                query += string.Format("from his_patient_type_alter pta\n");
                query += string.Format("join his_department_tran dpt on dpt.id=pta.department_tran_id \n");
                query += string.Format("where 1=1\n");
                query += string.Format("and pta.treatment_type_id in ({0})\n", string.Join(",", new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU }));
                query += string.Format("and dpt.department_id in ({0})\n", string.Join(",", KccDepartmentIds));
                query += string.Format("group by\n");
                query += string.Format("pta.treatment_id\n");
                query += string.Format(") pta on pta.treatment_id=trea.id \n");

                query += "where 1 = 1 \n";
                query += "and trea.in_room_id is not null \n";


                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.tdl_treatment_type_id in ({0})\n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }

                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }
                if (filter.BRANCH_IDs != null)
                {
                    query += string.Format("and trea.branch_id in ({0})\n", string.Join(",", filter.BRANCH_IDs));
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<TREATMENT>(query);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<CountWithPatientType> CountExamWait(Mrs00002Filter filter)
        {
            List<CountWithPatientType> result = null;
            try
            {
                string query = "";
                query += "SELECT \n";
                query += "sr.execute_room_id as Id, \n";
                query += "sum(case when trea.tdl_patient_type_id =1 then 1 else 0 end) as CountBhyt, \n";
                query += "sum(case when trea.tdl_patient_type_id <>1 then 1 else 0 end) as CountVp \n";

                query += "from his_rs.his_service_req sr\n";
                query += "join his_rs.his_treatment trea on sr.treatment_id = trea.id\n";
                query += "join his_rs.v_his_room exro on sr.execute_room_id = exro.id\n";
                query += "join his_rs.his_room rero on sr.request_room_id = rero.id\n";

                query += "WHERE sr.is_no_execute is null\n";
                query += "and sr.service_req_type_id =1\n";
                query += "and sr.is_delete =0 and rero.room_type_id = 3 and sr.service_req_stt_id =1 \n";
                query += string.Format("and SR.INTRUCTION_TIME between {0} and {1}\n", filter.INTRUCTION_TIME_FROM ?? filter.CREATE_TIME_FROM, filter.INTRUCTION_TIME_TO ?? filter.CREATE_TIME_TO);


                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.tdl_treatment_type_id in ({0})\n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }

                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }
                if (filter.BRANCH_IDs != null)
                {
                    query += string.Format("and trea.branch_id in ({0})\n", string.Join(",", filter.BRANCH_IDs));
                }
                query += "group by \n";
                query += "sr.execute_room_id \n";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<CountWithPatientType>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
        public List<CountWithPatientType> CountExamFinish(Mrs00002Filter filter)
        {
            List<CountWithPatientType> result = null;
            try
            {
                string query = "";
                query += "SELECT \n";
                query += "ro.Id\n, ";
                query += "sum(case when trea.tdl_patient_type_id =1 then 1 else 0 end) as CountBhyt, \n";
                query += "sum(case when trea.tdl_patient_type_id <>1 then 1 else 0 end) as CountVp \n";

                query += "from his_rs.his_treatment trea\n";
                query += "join his_rs.his_service_req sr on (sr.execute_room_id=trea.end_room_id and sr.treatment_id=trea.id and sr.service_req_stt_id=3 and sr.is_delete =0 and sr.is_no_execute is null and sr.service_req_type_id =1)\n";
                query += "join his_rs.v_his_room ro on trea.end_room_id = ro.id \n";

                query += "WHERE 1 = 1\n";
                query += string.Format("and trea.treatment_end_type_id in ({0}) \n", string.Join(",", new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET, 
                    IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN, 
                    IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CTCV, 
                    IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__HEN, 
                    IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__KHAC, 
                    IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__RAVIEN, 
                    IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__TRON, 
                    IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__XINRAVIEN }));
                query += string.Format("and trea.is_pause= {0}\n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                query += string.Format("and SR.FINISH_TIME between {0} and {1}\n", filter.INTRUCTION_TIME_FROM ?? filter.CREATE_TIME_FROM, filter.INTRUCTION_TIME_TO ?? filter.CREATE_TIME_TO);


                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.tdl_treatment_type_id in ({0})\n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }

                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }
                if (filter.BRANCH_IDs != null)
                {
                    query += string.Format("and trea.branch_id in ({0})\n", string.Join(",", filter.BRANCH_IDs));
                }
                query += "group by \n";
                query += "ro.ID \n";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<CountWithPatientType>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

         List<long> GetKccRoom()
        {
            List<long> result = new List<long>();
            try
            {
                string query = "select * from v_his_room where  INSTR((select ','||nvl(value,default_value)||',' from his_config where key = 'MRS.HIS_RS.HIS_ROOM.ROOM_CODE.KCC'),','||room_code||',')>0";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                List<V_HIS_ROOM> listRoomKCC = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_ROOM>(query);
                if (listRoomKCC != null && listRoomKCC.Count > 0)
                {
                    result = listRoomKCC.Select(o => o.ID).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new List<long>();
            }
            return result;
        }
         public List<OLD_PATIENT> GetOldPatient(Mrs00002Filter filter)
         {
             List<OLD_PATIENT> result = null;
             try
             {
                 string query = "";
                 query += string.Format("--du lieu ho so dieu tri cu tai kham\n");
                 query += string.Format("select \n");
                 query += string.Format("trea.id treatment_id\n");
                 query += string.Format("from his_treatment trea\n");
                 query += string.Format("join his_treatment old on old.patient_id=trea.patient_id and old.id<trea.id\n");
                 query += string.Format("left join his_branch br on br.id=trea.branch_id\n");
                 query += string.Format("join \n");
                 query += string.Format("( \n");
                 query += string.Format("select \n");
                 query += string.Format("sr.treatment_id\n");
                 query += string.Format("from his_service_req sr\n");
                 query += string.Format("join v_his_room ro on ro.id=sr.request_room_id\n");
                
                 query += string.Format("where 1=1\n");
                 if (filter.EXAM_ROOM_IDs != null)
                 {
                     query += string.Format("and sr.execute_room_id in ({0})\n", string.Join(",", filter.EXAM_ROOM_IDs));
                 }
                 if (filter.DEPARTMENT_IDs != null)
                 {
                     query += string.Format("and sr.execute_department_id in ({0})\n", string.Join(",", filter.DEPARTMENT_IDs));
                 }
                 query += string.Format("and (ro.room_type_id ={0} or ro.is_exam ={1})\n", IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);

                 if (filter.CREATE_TIME_FROM > 0)
                 {
                     query += string.Format("and sr.create_time >= {0}\n", filter.CREATE_TIME_FROM);
                 }
                 if (filter.CREATE_TIME_TO > 0)
                 {
                     query += string.Format("and sr.create_time <= {0}\n", filter.CREATE_TIME_TO);
                 }

                 if (filter.IS_BILL_TIME == true)
                 {
                     if (filter.INTRUCTION_TIME_FROM > 0 && filter.INTRUCTION_TIME_TO > 0)
                     {
                         query += string.Format("and exists(select 1 from his_sere_serv_bill ssb where ssb.tdl_service_req_id=sr.id and ssb.is_cancel is null and ssb.create_time between {0} and {1} or select 1 from his_sere_serv_deposit ssd where ssd.tdl_service_req_id=sr.id and ssd.is_cancel is null and ssd.create_time between {0} and {1})\n", filter.INTRUCTION_TIME_FROM, filter.INTRUCTION_TIME_TO);
                     }

                 }
                 else
                 {
                     if (filter.IS_INTRUCTION_TIME_OR_FINISH_TIME == false)
                     {
                         if (filter.INTRUCTION_TIME_FROM > 0)
                         {
                             query += string.Format("and sr.finish_time >= {0}\n", filter.INTRUCTION_TIME_FROM);
                         }
                         if (filter.INTRUCTION_TIME_TO > 0)
                         {
                             query += string.Format("and sr.finish_time <= {0}\n", filter.INTRUCTION_TIME_TO);
                         }
                     }
                     else
                     {
                         if (filter.INTRUCTION_TIME_FROM > 0)
                         {
                             query += string.Format("and sr.intruction_time >= {0}\n", filter.INTRUCTION_TIME_FROM);
                         }
                         if (filter.INTRUCTION_TIME_TO > 0)
                         {
                             query += string.Format("and sr.intruction_time <= {0}\n", filter.INTRUCTION_TIME_TO);
                         }
                     }
                 }
                 query += string.Format("and sr.is_no_execute is null\n");
                 query += string.Format("and sr.is_delete=0\n");
                 query += string.Format("and sr.service_req_type_id=1\n");
                 query += string.Format("group by\n");
                 query += string.Format("sr.treatment_id\n");
                 query += string.Format(") sr on sr.treatment_id=trea.id \n");

                 query += string.Format("where 1=1\n");

                 if (filter.TREATMENT_TYPE_IDs != null)
                 {
                     query += string.Format("and trea.tdl_treatment_type_id in ({0})\n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                 }

                 if (filter.TDL_PATIENT_TYPE_IDs != null)
                 {
                     query += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                 }
                 if (filter.BRANCH_IDs != null)
                 {
                     query += string.Format("and trea.branch_id in ({0})\n", string.Join(",", filter.BRANCH_IDs));
                 }
                 Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                 result = new MOS.DAO.Sql.SqlDAO().GetSql<OLD_PATIENT>(query);
             }
             catch (Exception ex)
             {
                 Inventec.Common.Logging.LogSystem.Error(ex);
                 result = null;
             }
             return result;
         }

         public List<CountWithPatientClassify> CountPatientClassify(Mrs00002Filter filter)
         {
             List<CountWithPatientClassify> result = null;
             try
             {
                 string query = "";
                 
                 query += string.Format("select \n");
                 query += string.Format("sr.tdl_patient_classify_id, \n");
                 query += string.Format("pc.patient_classify_code, \n");
                 query += string.Format("nvl(pc.patient_classify_name,'none') as PATIENT_CLASSIFY_NAME, \n");
                 query += string.Format("sr.execute_room_id, \n");
                 query += string.Format("sum(case when sr.tdl_patient_classify_id is not null then 1 else 0 end) as COUNT_CLASSIFY \n");
                 query += string.Format("from his_service_req sr\n");
                 query += string.Format("left join his_sere_serv ss on sr.id = ss.service_req_id\n");
                
                 query += string.Format("join v_his_room ro on ro.id=sr.request_room_id\n");
                 query += string.Format("join his_patient_classify pc on sr.tdl_patient_classify_id = pc.id\n");
                 query += string.Format("where 1=1\n");
                 if (filter.EXAM_ROOM_IDs != null)
                 {
                     query += string.Format("and sr.execute_room_id in ({0})\n", string.Join(",", filter.EXAM_ROOM_IDs));
                 }
                 if (filter.DEPARTMENT_IDs != null)
                 {
                     query += string.Format("and sr.execute_department_id in ({0})\n", string.Join(",", filter.DEPARTMENT_IDs));
                 }
                 query += string.Format("and (ro.room_type_id ={0} or ro.is_exam ={1})\n", IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                 if (filter.CREATE_TIME_FROM > 0)
                 {
                     query += string.Format("and sr.create_time >= {0}\n", filter.CREATE_TIME_FROM);
                 }
                 if (filter.CREATE_TIME_TO > 0)
                 {
                     query += string.Format("and sr.create_time <= {0}\n", filter.CREATE_TIME_TO);
                 }

                 if (filter.IS_BILL_TIME == true)
                 {
                     if (filter.INTRUCTION_TIME_FROM > 0 && filter.INTRUCTION_TIME_TO > 0)
                     {
                         query += string.Format("and exists(select 1 from his_sere_serv_bill ssb where ssb.tdl_service_req_id=sr.id and ssb.is_cancel is null and ssb.create_time between {0} and {1} or select 1 from his_sere_serv_deposit ssd where ssd.tdl_service_req_id=sr.id and ssd.is_cancel is null and ssd.create_time between {0} and {1})\n", filter.INTRUCTION_TIME_FROM, filter.INTRUCTION_TIME_TO);
                     }

                 }
                 else
                 {
                     if (filter.IS_INTRUCTION_TIME_OR_FINISH_TIME == false)
                     {
                         if (filter.INTRUCTION_TIME_FROM > 0)
                         {
                             query += string.Format("and sr.finish_time >= {0}\n", filter.INTRUCTION_TIME_FROM);
                         }
                         if (filter.INTRUCTION_TIME_TO > 0)
                         {
                             query += string.Format("and sr.finish_time <= {0}\n", filter.INTRUCTION_TIME_TO);
                         }
                     }
                     else
                     {
                         if (filter.INTRUCTION_TIME_FROM > 0)
                         {
                             query += string.Format("and sr.intruction_time >= {0}\n", filter.INTRUCTION_TIME_FROM);
                         }
                         if (filter.INTRUCTION_TIME_TO > 0)
                         {
                             query += string.Format("and sr.intruction_time <= {0}\n", filter.INTRUCTION_TIME_TO);
                         }
                     }
                 }
                 query += string.Format("and sr.is_no_execute is null\n");
                 query += string.Format("and sr.is_delete=0\n");
                 query += string.Format("and sr.service_req_type_id=1\n");
                 query += string.Format("group by sr.tdl_patient_classify_id, pc.patient_classify_code, pc.patient_classify_name, sr.execute_room_id \n");
                 query += string.Format("order by sr.tdl_patient_classify_id\n");
                 Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                 result = new MOS.DAO.Sql.SqlDAO().GetSql<CountWithPatientClassify>(query);
             }
             catch (Exception ex)
             {
                 Inventec.Common.Logging.LogSystem.Error(ex);
                 result = null;
             }
             return result;
         }
    }

    public class CountWithPatientType
    {
        public long ID { get; set; }
        public long? CountBhyt { get; set; }
        public long? CountVp { get; set; }
    }

    public class CountWithPatientClassify
    {
        public long? TDL_PATIENT_CLASSIFY_ID { get; set; }
        public string PATIENT_CLASSIFY_CODE { get; set; }
        public string PATIENT_CLASSIFY_NAME { get; set; }
        public long? COUNT_CLASSIFY { get; set; }
        public long EXECUTE_ROOM_ID { get; set; }
    }

    public class SERVICE_REQ
    {
        public long ID { get; set; }
        public long? TDL_PATIENT_CLASSIFY_ID { get; set; }
        public long EXECUTE_ROOM_ID { get; set; }
        public long REQUEST_ROOM_ID { get; set; }
        public long TREATMENT_ID { get; set; }
        public long INTRUCTION_TIME { get; set; }
        public long INTRUCTION_DATE { get; set; }
        public long? PREVIOUS_SERVICE_REQ_ID { get; set; }
        public long? TDL_PATIENT_GENDER_ID { get; set; }
        public short? IS_MAIN_EXAM { get; set; }
        public short? IS_EMERGENCY { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public long SERVICE_REQ_STT_ID { get; set; }
        public long? FINISH_TIME { get; set; }
        public double? HEIN_RATIO { get; set; }
        public string TDL_PATIENT_ADDRESS { set; get; }
    }

    public class TREATMENT
    {
        public long ID { get; set; }
        public long? TDL_PATIENT_CLASSIFY_ID { get; set; }
        public long? APPOINTMENT_TIME { get; set; }
        public string TDL_HEIN_CARD_NUMBER { get; set; }
        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public long? IN_ROOM_ID { get; set; }
        public long? IN_TREATMENT_TYPE_ID { get; set; }
        public long? END_DEPARTMENT_ID { get; set; }
        public long? OUT_DATE { get; set; }
        public short? IS_PAUSE { get; set; }
        public long? TREATMENT_END_TYPE_ID { get; set; }
        public long? OUT_TIME { get; set; }
        public long? CLINICAL_IN_TIME { get; set; }
        public long? END_ROOM_ID { get; set; }
        public long? TRAN_PATI_FORM_ID { get; set; }
        public short? IS_IN_PROVINCE { get; set; }
        public long? TDL_KSK_CONTRACT_ID { get; set; }
        public string TDL_PATIENT_NATIONAL_NAME { get; set; }
        public long PATIENT_ID { get; set; }
        //public long? IS_EMERGENCY { get; set; }
        //public string CAP_CUU { get; set; }


    }
    public class PATIENT_TYPE_ALTER
    {
        public long TREATMENT_ID { get; set; }
        public long LOG_TIME { get; set; }
        public string RIGHT_ROUTE_CODE { get; set; }

        public long PATIENT_TYPE_ID { get; set; }
    }
    public class OLD_PATIENT
    {
        public long TREATMENT_ID { get; set; }
    }
}

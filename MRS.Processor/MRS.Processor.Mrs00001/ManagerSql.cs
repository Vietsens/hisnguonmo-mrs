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
using MOS.MANAGER.HisSereServ;
using SDA.EFMODEL.DataModels;


namespace MRS.Processor.Mrs00001
{
    public class ManagerSql
    {

        public List<Mrs00001RDO> GetServiceReq(Mrs00001Filter filter, List<V_SDA_PROVINCE> listProvince, List<V_SDA_DISTRICT> listDistrict, List<V_SDA_COMMUNE> listCommune, List<HIS_ICD> listICD)
        {

            List<Mrs00001RDO> result = null;
            try
            {

                string query = "";
                query += string.Format("--danh sach y lenh kham\n");
                query += string.Format("select \n");
                //them trường thời gian lọc báo cáo
                if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
                {
                    query += "sr.intruction_time filter_time,\n";
                    query += "round(sr.intruction_time,-8) filter_month,\n";
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    query += "sr.START_TIME filter_time,\n";
                    query += "round(sr.START_TIME,-8) filter_month,\n";
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    query += "sr.FINISH_TIME filter_time,\n";
                    query += "round(sr.FINISH_TIME,-8) filter_month,\n";
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    query += "trea.IN_TIME filter_time,\n";
                    query += "round(trea.IN_TIME,-8) filter_month,\n";
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
                {
                    query += "trea.CLINICAL_IN_TIME filter_time,\n";
                    query += "round(trea.CLINICAL_IN_TIME,-8) filter_month,\n";
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    query += "trea.OUT_TIME filter_time,\n";
                    query += "round(trea.OUT_TIME,-8) filter_month,\n";
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
                {
                    query += "trea.FEE_LOCK_TIME filter_time,\n";
                    query += "round(trea.FEE_LOCK_TIME,-8) filter_month,\n";
                }
                else
                {
                    query += "sr.intruction_time filter_time,\n";
                    query += "round(sr.intruction_time,-8) filter_month,\n";
                }
                query += string.Format("sr.previous_service_req_id,\n");
                query += string.Format("trea.is_emergency,\n");
                query += string.Format("sr.service_req_stt_id,\n");
                query += "sr.patient_case_id,\n";
                query += "sr.tdl_patient_address vir_address,\n";
                query += "trea.transfer_in_medi_org_code,\n";
                query += "trea.transfer_in_medi_org_name,\n";
                query += "trea.tdl_hein_card_number hein_card_number,\n";
                query += "trea.medi_org_name,\n";
                query += "sr.create_time,\n";
                query += "sr.id,\n";
                query += "sr.tdl_service_ids,\n";
                query += "sr.intruction_time,\n";
                query += "sr.treatment_id,\n";
                query += "sr.execute_room_id,\n";
                query += "sr.icd_code,\n";
                query += "sr.icd_name,\n";
                query += "sr.icd_sub_code,\n";
                query += "sr.icd_text,\n";
                query += "sr.execute_loginname,\n";
                query += "sr.execute_username,\n";
                query += "sr.finish_time,\n";
                query += "sr.icd_cause_code,\n";
                query += "sr.icd_cause_name,\n";
                query += "sr.intruction_date,\n";
                query += "sr.is_main_exam,\n";
                query += "sr.request_loginname, \n";
                query += "sr.request_room_id,\n";
                query += "sr.request_username,\n";
                query += "sr.request_department_id,\n";
                query += "sr.service_req_code,\n";
                query += "sr.service_req_stt_id,\n";
                query += "sr.service_req_type_id, \n";
                query += "sr.session_code,\n";
                query += "sr.sick_day,\n";
                query += "sr.start_time,\n";
                query += "sr.is_not_require_fee,\n";
                query += "sr.tracking_id,\n";
                query += "sr.treatment_instruction,\n";
                query += "sr.treatment_type_id,\n";
                query += "nvl(sr.tdl_patient_mobile,nvl(trea.tdl_patient_mobile,pt.mobile)) tdl_patient_mobile,\n";
                query += "sr.tdl_patient_career_name,\n";
                query += "sr.tdl_patient_code,\n";
                query += "sr.tdl_patient_commune_code,\n";
                query += "nvl(sr.tdl_patient_phone,nvl(trea.tdl_patient_phone,pt.phone)) tdl_patient_phone,\n";
                query += "nvl(pt.relative_mobile,trea.TDL_PATIENT_RELATIVE_MOBILE) relative_mobile,\n";
                query += "nvl(pt.relative_phone,trea.TDL_PATIENT_RELATIVE_PHONE) relative_phone,\n";
                query += "pt.ethnic_name,\n";
                query += "sr.tdl_patient_district_code,\n";
                query += "sr.tdl_patient_dob,\n";
                query += "sr.tdl_patient_gender_id,\n";
                query += "sr.tdl_patient_id,\n";
                query += "sr.tdl_patient_military_rank_name,\n";
                query += "sr.tdl_patient_name,\n";
                query += "sr.tdl_patient_national_name,\n";
                query += "sr.tdl_patient_province_code,\n";
                query += "sr.tdl_patient_work_place,\n";
                query += "sr.tdl_patient_work_place_name,\n";
                query += "sr.tdl_hein_card_number,\n";
                query += "sr.tdl_hein_medi_org_code,\n";
                query += "sr.tdl_hein_medi_org_name,\n";
                query += "sr.tdl_treatment_code,\n";
                query += "trea.in_room_id,\n";
                query += "trea.end_room_id,\n";
                query += "trea.tdl_first_exam_room_id,\n";
                query += "trea.is_pause,\n";
                query += "trea.tdl_patient_address,\n";
                query += "trea.tdl_patient_type_id,\n";
                query += "trea.tdl_treatment_type_id,\n";
                query += "trea.transfer_in_icd_name,\n";
                query += "trea.transfer_in_icd_code,\n";
                query += "trea.clinical_in_time,\n";
                query += "trea.medi_org_code,\n";
                query += "trea.treatment_end_type_id,\n";
                query += "trea.in_treatment_type_id,\n";
                query += "trea.end_department_id,\n";
                query += "SF.SERVICE_GROUP_NAME,\n";
                
                if (filter.IS_UPDATE_ADDRESS == true)
                {
                    query += "pta.address,\n";
                }

                query += "trea.tran_pati_form_id,\n";
                query += "trea.tran_pati_reason_id\n";
                query += string.Format("from his_service_req sr\n");
                query += string.Format("join v_his_room ro on ro.id=sr.request_room_id\n");
                query += string.Format("join lateral(select 1 from his_treatment trea where trea.id=sr.treatment_id) trea on trea.id=sr.treatment_id \n");
                query += string.Format("join his_patient pt on pt.id=trea.patient_id \n");
                query += string.Format("join his_sere_serv ss on ss.SERVICE_REQ_ID = sr.id \n");
                query += string.Format("JOIN HIS_SERVICE S ON S.ID = SS.SERVICE_ID \n");
                query += string.Format("LEFT JOIN HIS_SERV_SEGR SEG ON S.ID = SEG.SERVICE_ID \n");
                query += string.Format("LEFT JOIN HIS_SERVICE_GROUP SF ON SF.ID = SEG.SERVICE_GROUP_ID \n");

                
               
               
            

                if (filter.IS_UPDATE_ADDRESS == true)
                {
                    query += string.Format("left join his_patient_type_alter pta on pta.treatment_id=trea.id and pta.treatment_type_id=1 and trea.tdl_patient_address is null \n");
                }
                query += string.Format("where 1=1\n");
                query += string.Format("and sr.is_delete=0\n");
                query += string.Format("and sr.is_no_execute is null\n");
                query += string.Format("and sr.service_req_type_id={0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH);

                if (filter.INPUT_DATA_ID_TIME_TYPE == 1 || filter.INPUT_DATA_ID_TIME_TYPE == null || filter.INPUT_DATA_ID_TIME_TYPE > 7 || filter.INPUT_DATA_ID_TIME_TYPE <1)
                {
                    FilterTime(ref query, "sr.intruction_time", filter);
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    FilterTime(ref query, "sr.start_time", filter);
                    
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    FilterTime(ref query, "sr.finish_time", filter);
                    query += string.Format("and sr.service_req_stt_id ={0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    FilterTime(ref query, "trea.in_time", filter);
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
                {
                    FilterTime(ref query, "trea.clinical_in_time", filter);
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    FilterTime(ref query, "trea.out_time", filter);
                    query += string.Format("and trea.is_pause ={0}\n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
                {
                    FilterTime(ref query, "trea.fee_lock_time", filter);
                    query += string.Format("and trea.is_active ={0}\n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                }
                if (filter.EXAM_ROOM_ID != null)
                {
                    query += string.Format("and sr.execute_room_id ={0}\n", filter.EXAM_ROOM_ID);
                }
                if (filter.EXAM_ROOM_IDs != null)
                {
                    query += string.Format("and sr.execute_room_id in ({0})\n", string.Join(",", filter.EXAM_ROOM_IDs));
                }
                if (filter.REQUEST_ROOM_ID != null)
                {
                    query += string.Format("and sr.request_room_id = {0}\n", filter.REQUEST_ROOM_ID);
                }
                if (filter.REQUEST_ROOM_IDs != null)
                {
                    query += string.Format("and sr.request_room_id in({0})\n", string.Join(",", filter.REQUEST_ROOM_IDs));
                }
                if (filter.IS_FINISH == true)
                {
                    query += string.Format("and sr.service_req_stt_id = {0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
                query += string.Format("and (ro.room_type_id ={0} or ro.is_exam ={1})\n", IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                if (filter.PATIENT_TYPE_ID != null)
                {
                    query += string.Format("and trea.tdl_patient_type_id ={0}\n", filter.PATIENT_TYPE_ID);
                }
                
                if (filter.ICD_IDs != null)
                {
                    List<string> icd = listICD != null ? listICD.Where(p => filter.ICD_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.ICD_CODE)).Select(p => p.ICD_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.icd_code in ('{0}')\n", string.Join("','", icd));

                }

                if (filter.PROVINCE_IDs != null)
                {
                    List<string> provinceCodes = listProvince != null ? listProvince.Where(p => filter.PROVINCE_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.PROVINCE_CODE)).Select(o => o.PROVINCE_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.tdl_patient_province_code in ('{0}')\n", string.Join("','", provinceCodes));
                }
                if (filter.DISTRICT_IDs != null)
                {
                    List<string> districtCodes = listDistrict != null ? listDistrict.Where(p => filter.DISTRICT_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.DISTRICT_CODE)).Select(o => o.DISTRICT_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.tdl_patient_district_code in ('{0}')\n", string.Join("','", districtCodes));
                }
                if (filter.COMMUNE_IDs != null)
                {
                    List<string> communeCodes = listCommune != null ? listCommune.Where(p => filter.COMMUNE_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.COMMUNE_CODE)).Select(o => o.COMMUNE_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.tdl_patient_commune_code in ('{0}')\n", string.Join("','", communeCodes));
                }
                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0}) \n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00001RDO>(query);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        private void FilterTime(ref string query,string stringTime, Mrs00001Filter filter)
        {
            if (filter.INTRUCTION_TIME_FROM != null)
            {
                query += string.Format("and {2} between {0} and {1}\n", filter.INTRUCTION_TIME_FROM, filter.INTRUCTION_TIME_TO, stringTime);
                if (filter.HOUR_TIME_FROM > filter.HOUR_TIME_TO && filter.HOUR_TIME_TO > 0)
                {
                    query += string.Format("and mod({2},1000000) not between {0} and {1}\n", filter.HOUR_TIME_TO, filter.HOUR_TIME_FROM, stringTime);
                }
                else
                {
                    query += filter.HOUR_TIME_FROM > 0 ? string.Format("and mod({1},1000000) >={0}\n", filter.HOUR_TIME_FROM, stringTime) : "";
                    query += filter.HOUR_TIME_TO > 0 ? string.Format("and mod({1},1000000) <={0}\n", filter.HOUR_TIME_TO, stringTime) : "";
                }//addTime1
            }
        }

        public List<SERE_SERV> GetSereServ(Mrs00001Filter filter, List<V_SDA_PROVINCE> listProvince, List<V_SDA_DISTRICT> listDistrict, List<V_SDA_COMMUNE> listCommune, List<HIS_ICD> listICD)
        {

            List<SERE_SERV> result = null;
            try
            {
                string query = "";
                query += string.Format("--danh sach dich vu cua benh nhan kham\n");
                query += string.Format("select \n");
                query += string.Format("ss.id,\n");
                query += "ss.service_req_id, \n";
                query += "ss.tdl_treatment_id, \n";
                query += "ss.tdl_request_room_id, \n";
                query += "ss.service_id, \n";
                query += "ss.tdl_service_type_id, \n";
                query += "ss.tdl_service_name, \n";
                query += "ss.amount \n";
                query += string.Format("from his_sere_serv ss\n");
                query += string.Format("join (\n");
                query += string.Format("select \n");

                query += string.Format("trea.id\n");
                query += string.Format("from his_service_req sr\n");
                query += string.Format("join v_his_room ro on ro.id=sr.request_room_id\n");
                query += string.Format("join his_treatment trea on trea.id=sr.treatment_id \n");
                query += string.Format("join his_patient pt on pt.id = trea.patient_id\n");
                query += string.Format("where 1=1\n");
                query += string.Format("and sr.is_delete=0\n");
                query += string.Format("and sr.is_no_execute is null\n");
                query += string.Format("and sr.service_req_type_id={0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH);
                if (filter.INPUT_DATA_ID_TIME_TYPE == 1 || filter.INPUT_DATA_ID_TIME_TYPE == null || filter.INPUT_DATA_ID_TIME_TYPE > 7 || filter.INPUT_DATA_ID_TIME_TYPE < 1)
                {
                    FilterTime(ref query, "sr.intruction_time", filter);
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    FilterTime(ref query, "sr.start_time", filter);
                    
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    FilterTime(ref query, "sr.finish_time", filter);
                    query += string.Format("and sr.service_req_stt_id ={0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
                if (filter.DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND sr.request_department_id in ({0})\n", string.Join(",", filter.DEPARTMENT_IDs));
                }
                if (filter.EXAM_ROOM_ID != null)
                {
                    query += string.Format("and sr.execute_room_id ={0}\n", filter.EXAM_ROOM_ID);
                }
                if (filter.EXAM_ROOM_IDs != null)
                {
                    query += string.Format("and sr.execute_room_id in ({0})\n", string.Join(",", filter.EXAM_ROOM_IDs));
                }
                if (filter.REQUEST_ROOM_ID != null)
                {
                    query += string.Format("and sr.request_room_id = {0}\n", filter.REQUEST_ROOM_ID);
                }
                if (filter.REQUEST_ROOM_IDs != null)
                {
                    query += string.Format("and sr.request_room_id in({0})\n", string.Join(",", filter.REQUEST_ROOM_IDs));
                }
                if (filter.IS_FINISH == true)
                {
                    query += string.Format("and sr.service_req_stt_id = {0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
                query += string.Format("and (ro.room_type_id ={0} or ro.is_exam ={1})\n", IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);

                if (filter.ICD_IDs != null)
                {
                    List<string> icd = listICD != null ? listICD.Where(p => filter.ICD_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.ICD_CODE)).Select(p => p.ICD_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.icd_code in ('{0}')\n", string.Join("','", icd));

                }

                if (filter.PROVINCE_IDs != null)
                {
                    List<string> provinceCodes = listProvince != null ? listProvince.Where(p => filter.PROVINCE_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.PROVINCE_CODE)).Select(o => o.PROVINCE_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.tdl_patient_province_code in ('{0}')\n", string.Join("','", provinceCodes));
                }
                if (filter.DISTRICT_IDs != null)
                {
                    List<string> districtCodes = listDistrict != null ? listDistrict.Where(p => filter.DISTRICT_IDs.Contains(p.ID) && !string.IsNullOrWhiteSpace(p.DISTRICT_CODE)).Select(o => o.DISTRICT_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.tdl_patient_district_code in ('{0}')", string.Join("','", districtCodes));
                }
                if (filter.COMMUNE_IDs != null)
                {
                    List<string> communeCodes = listCommune != null ? listCommune.Where(p => filter.COMMUNE_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.COMMUNE_CODE)).Select(o => o.COMMUNE_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.tdl_patient_commune_code in ('{0}')\n", string.Join("','", communeCodes));
                }
                if (filter.PATIENT_TYPE_ID != null)
                {
                    query += string.Format("and trea.tdl_patient_type_id ={0}\n", filter.PATIENT_TYPE_ID);
                }
                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0}) \n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    FilterTime(ref query, "trea.in_time", filter);
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
                {
                    FilterTime(ref query, "trea.clinical_in_time", filter);
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    FilterTime(ref query, "trea.out_time", filter);
                    query += string.Format("and trea.is_pause ={0}\n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
                {
                    FilterTime(ref query, "trea.fee_lock_time", filter);
                    query += string.Format("and trea.is_active ={0}\n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                }

                query += string.Format(") trea on trea.id=ss.tdl_treatment_id\n");
                query += string.Format("where 1=1\n");
                query += string.Format("and ss.is_delete=0\n");
                query += string.Format("and ss.is_no_execute is null\n");

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<SERE_SERV>(query);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<Mrs00001RDO> GetServiceReqOutOfKCC_KKB(Mrs00001Filter filter, List<long> KccDepartmentIds, List<V_SDA_PROVINCE> listProvince, List<V_SDA_DISTRICT> listDistrict, List<V_SDA_COMMUNE> listCommune, List<HIS_ICD> listICD)
        {

            List<Mrs00001RDO> result = null;
            try
            {
                if (KccDepartmentIds == null || KccDepartmentIds.Count == 0)
                {
                    return null;
                }
                string query = "--du lieu y lenh kham vao noi tru va ra khoi khoa kham benh cap cuu\n";
                query += "SELECT \n";
                query += string.Format("sr.previous_service_req_id,\n");
                query += string.Format("sr.is_emergency,\n");
                query += string.Format("sr.service_req_stt_id,\n");
                query += "sr.patient_case_id,\n";
                query += "sr.tdl_patient_address vir_address,\n";
                query += "trea.transfer_in_medi_org_code,\n";
                query += "trea.transfer_in_medi_org_name,\n";
                query += "trea.tdl_hein_card_number hein_card_number,\n";
                query += "trea.medi_org_name,\n";
                query += "sr.create_time,\n";
                query += "sr.id,\n";
                query += "sr.intruction_time,\n";
                query += "sr.treatment_id,\n";
                query += "sr.execute_room_id,\n";
                query += "sr.icd_code,\n";
                query += "sr.icd_name,\n";
                query += "sr.icd_sub_code,\n";
                query += "sr.icd_text,\n";
                query += "sr.execute_loginname,\n";
                query += "sr.execute_username,\n";
                query += "sr.finish_time,\n";
                query += "sr.icd_cause_code,\n";
                query += "sr.icd_cause_name,\n";
                query += "sr.intruction_date,\n";
                query += "sr.is_main_exam,\n";
                query += "sr.request_loginname, \n";
                query += "sr.request_room_id,\n";
                query += "sr.request_username,\n";
                query += "sr.request_department_id,\n";
                query += "sr.service_req_code,\n";
                query += "sr.service_req_stt_id,\n";
                query += "sr.service_req_type_id, \n";
                query += "sr.session_code,\n";
                query += "sr.sick_day,\n";
                query += "sr.start_time,\n";
                query += "sr.is_not_require_fee,\n";
                query += "sr.tracking_id,\n";
                query += "sr.treatment_instruction,\n";
                query += "sr.treatment_type_id,\n";
                query += "nvl(sr.tdl_patient_mobile,nvl(trea.tdl_patient_mobile,pt.mobile)) tdl_patient_mobile,\n";
                query += "nvl(pt.relative_mobile,trea.TDL_PATIENT_RELATIVE_MOBILE) relative_mobile,\n";
                query += "nvl(pt.relative_phone,trea.TDL_PATIENT_RELATIVE_PHONE) relative_phone,\n";
                query += "sr.tdl_patient_career_name,\n";
                query += "sr.tdl_patient_code,\n";
                query += "sr.tdl_patient_commune_code,\n";
                query += "nvl(sr.tdl_patient_phone,nvl(trea.tdl_patient_phone,pt.phone)) tdl_patient_phone,\n";
                query += "pt.ethnic_name,\n";
                query += "sr.tdl_patient_district_code,\n";
                query += "sr.tdl_patient_dob,\n";
                query += "sr.tdl_patient_gender_id,\n";
                query += "sr.tdl_patient_id,\n";
                query += "sr.tdl_patient_military_rank_name,\n";
                query += "sr.tdl_patient_name,\n";
                query += "sr.tdl_patient_national_name,\n";
                query += "sr.tdl_patient_province_code,\n";
                query += "sr.tdl_patient_work_place,\n";
                query += "sr.tdl_patient_work_place_name,\n";
                query += "sr.tdl_hein_card_number,\n";
                query += "sr.tdl_hein_medi_org_code,\n";
                query += "sr.tdl_hein_medi_org_name,\n";
                query += "sr.tdl_treatment_code,\n";
                query += "trea.in_room_id,\n";
                query += "trea.end_room_id,\n";
                query += "trea.tdl_first_exam_room_id,\n";
                query += "trea.is_pause,\n";
                query += "trea.tdl_patient_address,\n";
                query += "trea.tdl_patient_type_id,\n";
                query += "trea.tdl_treatment_type_id,\n";
                query += "trea.transfer_in_icd_name,\n";
                query += "trea.transfer_in_icd_code,\n";
                query += "trea.clinical_in_time,\n";
                query += "trea.medi_org_code,\n";
                query += "trea.treatment_end_type_id,\n";
                query += "trea.in_treatment_type_id,\n";
                query += "trea.end_department_id,\n";
                query += "SF.SERVICE_GROUP_NAME,\n";
                query += "trea.tran_pati_form_id\n";

                query += "from his_service_req sr \n";
                query += string.Format("join v_his_room ro on ro.id=sr.request_room_id\n");
                query += string.Format("join his_sere_serv ss on ss.SERVICE_REQ_ID = sr.id \n");
                query += string.Format("JOIN HIS_SERVICE S ON S.ID = SS.SERVICE_ID \n");
                query += string.Format("LEFT JOIN HIS_SERV_SEGR SEG ON S.ID = SEG.SERVICE_ID \n");
                query += string.Format("LEFT JOIN HIS_SERVICE_GROUP SF ON SF.ID = SEG.SERVICE_GROUP_ID \n");
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
                query += string.Format("and (case when next.department_in_time>0 then next.department_in_time when trea.is_pause = {2} and trea.end_department_id=dpt.department_id then trea.out_time else {0}-1 end) between {0} and {1}\n", filter.INTRUCTION_TIME_FROM ?? filter.FINISH_TIME_FROM ?? filter.IN_TIME_FROM ?? filter.OUT_TIME_FROM ?? filter.FEE_LOCK_TIME_FROM, filter.INTRUCTION_TIME_TO ?? filter.FINISH_TIME_TO ?? filter.IN_TIME_TO ?? filter.OUT_TIME_TO ?? filter.FEE_LOCK_TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0}) \n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }
                if (filter.ICD_IDs != null)
                {
                    List<string> icd = listICD != null ? listICD.Where(p => filter.ICD_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.ICD_CODE)).Select(p => p.ICD_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.icd_code in ('{0}')\n", string.Join("','", icd));

                }

                if (filter.PROVINCE_IDs != null)
                {
                    List<string> provinceCodes = listProvince != null ? listProvince.Where(p => filter.PROVINCE_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.PROVINCE_CODE)).Select(o => o.PROVINCE_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.tdl_patient_province_code in ('{0}')\n", string.Join("','", provinceCodes));
                }
                if (filter.DISTRICT_IDs != null)
                {
                    List<string> districtCodes = listDistrict != null ? listDistrict.Where(p => filter.DISTRICT_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.DISTRICT_CODE)).Select(o => o.DISTRICT_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.tdl_patient_district_code in ('{0}')\n", string.Join("','", districtCodes));
                }
                if (filter.COMMUNE_IDs != null)
                {
                    List<string> communeCodes = listCommune != null ? listCommune.Where(p => filter.COMMUNE_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.COMMUNE_CODE)).Select(o => o.COMMUNE_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.tdl_patient_commune_code in ('{0}')\n", string.Join("','", communeCodes));
                }
                if (filter.EXAM_ROOM_ID != null)
                {
                    query += string.Format("AND trea.in_room_id = {0}\n", string.Join(",", filter.EXAM_ROOM_ID));
                }

                if (filter.EXAM_ROOM_IDs != null)
                {
                    query += string.Format("AND trea.in_room_id in({0}) \n", string.Join(",", filter.EXAM_ROOM_IDs));
                }
                query += string.Format("group by \n");
                query += string.Format("pta.treatment_id\n");
                query += string.Format(") pta on pta.treatment_id=sr.treatment_id \n");
                query += string.Format("join his_treatment trea on trea.id=sr.treatment_id \n");
                query += string.Format("join his_patient pt on pt.id=trea.patient_id \n");

                query += "where 1 = 1 \n";

                query += string.Format("and sr.is_no_execute is null\n");
                query += string.Format("and sr.is_delete=0\n");
                query += string.Format("and sr.service_req_type_id=1\n");
                if (filter.DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND sr.request_department_id in ({0})\n", string.Join(",", filter.DEPARTMENT_IDs));
                }
                if (filter.EXAM_ROOM_IDs != null)
                {
                    query += string.Format("and sr.execute_room_id in ({0})\n", string.Join(",", filter.EXAM_ROOM_IDs));
                }

                if (filter.EXAM_ROOM_ID != null)
                {
                    query += string.Format("AND sr.execute_room_id  = {0}\n", string.Join(",", filter.EXAM_ROOM_ID));
                }
                FilterTime(ref query, "sr.start_time", filter);
                
                query += string.Format("and (ro.room_type_id ={0} or ro.is_exam ={1})\n", IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00001RDO>(query);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<TREATMENT> GetByIdRemove(Mrs00001Filter filter, List<long> KccDepartmentIds, List<V_SDA_PROVINCE> listProvince, List<V_SDA_DISTRICT> listDistrict, List<V_SDA_COMMUNE> listCommune, List<HIS_ICD> listICD)
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
                query += string.Format("join his_patient pt on pt.id = sr.tdl_patient_id\n");
                query += string.Format("where 1=1\n");
                if (filter.INPUT_DATA_ID_TIME_TYPE == 1 || filter.INPUT_DATA_ID_TIME_TYPE == null || filter.INPUT_DATA_ID_TIME_TYPE > 7 || filter.INPUT_DATA_ID_TIME_TYPE < 1)
                {
                    FilterTime(ref query, "sr.intruction_time", filter);
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    FilterTime(ref query, "sr.start_time", filter);

                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    FilterTime(ref query, "sr.finish_time", filter);
                    query += string.Format("and sr.service_req_stt_id ={0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
                if (filter.EXAM_ROOM_ID != null)
                {
                    query += string.Format("and sr.execute_room_id ={0}\n", filter.EXAM_ROOM_ID);
                }
                if (filter.EXAM_ROOM_IDs != null)
                {
                    query += string.Format("and sr.execute_room_id in ({0})\n", string.Join(",", filter.EXAM_ROOM_IDs));
                }
                if (filter.REQUEST_ROOM_ID != null)
                {
                    query += string.Format("and sr.request_room_id = {0}\n", filter.REQUEST_ROOM_ID);
                }
                if (filter.REQUEST_ROOM_IDs != null)
                {
                    query += string.Format("and sr.request_room_id in({0})\n", string.Join(",", filter.REQUEST_ROOM_IDs));
                }
                if (filter.IS_FINISH == true)
                {
                    query += string.Format("and sr.service_req_stt_id = {0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
                query += string.Format("and (ro.room_type_id ={0} or ro.is_exam ={1})\n", IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
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
                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0}) \n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }
                if (filter.PATIENT_TYPE_ID != null)
                {
                    query += string.Format("and trea.tdl_patient_type_id ={0}\n", filter.PATIENT_TYPE_ID);
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    FilterTime(ref query, "trea.in_time", filter);
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
                {
                    FilterTime(ref query, "trea.clinical_in_time", filter);
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    FilterTime(ref query, "trea.out_time", filter);
                    query += string.Format("and trea.is_pause ={0}\n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
                {
                    FilterTime(ref query, "trea.fee_lock_time", filter);
                    query += string.Format("and trea.is_active ={0}\n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                }

                if (filter.ICD_IDs != null)
                {
                    List<string> icd = listICD != null ? listICD.Where(p => filter.ICD_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.ICD_CODE)).Select(p => p.ICD_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.icd_code in ('{0}')\n", string.Join("','", icd));

                }

                if (filter.PROVINCE_IDs != null)
                {
                    List<string> provinceCodes = listProvince != null ? listProvince.Where(p => filter.PROVINCE_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.PROVINCE_CODE)).Select(o => o.PROVINCE_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.tdl_patient_province_code in ('{0}')\n", string.Join("','", provinceCodes));
                }
                if (filter.DISTRICT_IDs != null)
                {
                    List<string> districtCodes = listDistrict != null ? listDistrict.Where(p => filter.DISTRICT_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.DISTRICT_CODE)).Select(o => o.DISTRICT_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.tdl_patient_district_code in ('{0}')\n", string.Join("','", districtCodes));
                }
                if (filter.COMMUNE_IDs != null)
                {
                    List<string> communeCodes = listCommune != null ? listCommune.Where(p => filter.COMMUNE_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.COMMUNE_CODE)).Select(o => o.COMMUNE_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.tdl_patient_commune_code in ('{0}')\n", string.Join("','", communeCodes));
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

        public List<SALE_MEDICINE> GetSaleExpMestMedicine(Mrs00001Filter filter, List<V_SDA_PROVINCE> listProvince, List<V_SDA_DISTRICT> listDistrict, List<V_SDA_COMMUNE> listCommune, List<HIS_ICD> listICD)
        {

            List<SALE_MEDICINE> result = null;
            try
            {

                string query = "--so luong thuoc benh nhan mua\n";
                query += "SELECT \n";
                query += "sr.treatment_id,\n ";
                query += "sr.request_room_id,\n ";
                query += "exmm.medicine_type_id, \n";
                query += "exmm.medicine_type_name, \n";
                query += "exmm.amount \n";

                query += string.Format("from his_rs.v_his_exp_mest_medicine exmm \n");
                query += string.Format("join his_rs.his_exp_mest ex on ex.id = exmm.exp_mest_id\n");
                query += string.Format("join his_rs.his_service_req sr on sr.id = ex.prescription_id\n");
                query += string.Format("join (\n");
                query += string.Format("select \n");
                query += string.Format("trea.id\n");
                query += string.Format("from his_treatment trea\n");
                query += string.Format("join \n");
                query += string.Format("(select\n");
                query += string.Format("sr.treatment_id\n");
                query += string.Format("from his_service_req sr \n");
                query += string.Format("join v_his_room ro on ro.id=sr.request_room_id\n");
                query += string.Format("join his_patient pt on pt.id = sr.tdl_patient_id\n");
                query += string.Format("where 1=1\n");
                query += string.Format("and sr.is_delete=0\n");
                query += string.Format("and sr.is_no_execute  is null\n");
                query += string.Format("and sr.service_req_type_id={0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH);
                if (filter.INPUT_DATA_ID_TIME_TYPE == 1 || filter.INPUT_DATA_ID_TIME_TYPE == null || filter.INPUT_DATA_ID_TIME_TYPE > 7 || filter.INPUT_DATA_ID_TIME_TYPE < 1)
                {
                    FilterTime(ref query, "sr.intruction_time", filter);
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    FilterTime(ref query, "sr.start_time", filter);

                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    FilterTime(ref query, "sr.finish_time", filter);
                    query += string.Format("and sr.service_req_stt_id ={0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
                if (filter.EXAM_ROOM_ID != null)
                {
                    query += string.Format("and sr.execute_room_id ={0}\n", filter.EXAM_ROOM_ID);
                }
                if (filter.EXAM_ROOM_IDs != null)
                {
                    query += string.Format("and sr.execute_room_id in({0})\n", string.Join(",", filter.EXAM_ROOM_IDs));
                }
                if (filter.REQUEST_ROOM_ID != null)
                {
                    query += string.Format("and sr.request_room_id ={0}\n", filter.REQUEST_ROOM_ID);
                }
                if (filter.REQUEST_ROOM_IDs != null)
                {
                    query += string.Format("and sr.request_room_id in({0})\n", string.Join(",", filter.REQUEST_ROOM_IDs));
                }
                if (filter.IS_FINISH == true)
                {
                    query += string.Format("and sr.service_req_stt_id = {0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
                query += string.Format("and (ro.room_type_id ={0} or ro.is_exam ={1})\n", IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);

                query += string.Format("group by\n");
                query += string.Format("sr.treatment_id\n");
                query += string.Format(") sr on sr.treatment_id = trea.id\n");
                query += string.Format("where 1=1\n");
                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0}) \n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }
                if (filter.PATIENT_TYPE_ID != null)
                {
                    query += string.Format("and trea.tdl_patient_type_id ={0}\n", filter.PATIENT_TYPE_ID);
                }
                if (filter.ICD_IDs != null)
                {
                    List<string> icd = listICD != null ? listICD.Where(p => filter.ICD_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.ICD_CODE)).Select(p => p.ICD_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.icd_code in ('{0}')\n", string.Join("','", icd));

                }

                if (filter.PROVINCE_IDs != null)
                {
                    List<string> provinceCodes = listProvince != null ? listProvince.Where(p => filter.PROVINCE_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.PROVINCE_CODE)).Select(o => o.PROVINCE_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.tdl_patient_province_code in ('{0}')\n", string.Join("','", provinceCodes));
                }
                if (filter.DISTRICT_IDs != null)
                {
                    List<string> districtCodes = listDistrict != null ? listDistrict.Where(p => filter.DISTRICT_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.DISTRICT_CODE)).Select(o => o.DISTRICT_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.tdl_patient_district_code in ('{0}')\n", string.Join("','", districtCodes));
                }
                if (filter.COMMUNE_IDs != null)
                {
                    List<string> communeCodes = listCommune != null ? listCommune.Where(p => filter.COMMUNE_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.COMMUNE_CODE)).Select(o => o.COMMUNE_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.tdl_patient_commune_code in ('{0}')\n", string.Join("','", communeCodes));
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    FilterTime(ref query, "trea.in_time", filter);
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
                {
                    FilterTime(ref query, "trea.clinical_in_time", filter);
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    FilterTime(ref query, "trea.out_time", filter);
                    query += string.Format("and trea.is_pause ={0}\n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
                {
                    FilterTime(ref query, "trea.fee_lock_time", filter);
                    query += string.Format("and trea.is_active ={0}\n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                }
                query += string.Format(") trea on trea.id=sr.treatment_id\n");
                query += string.Format("where 1=1\n");
                query += string.Format("and exmm.exp_mest_type_id=8\n");
                query += string.Format("and exmm.is_delete =0\n");
                query += string.Format("and exmm.is_export=1\n");
                result = new MOS.DAO.Sql.SqlDAO().GetSql<SALE_MEDICINE>(query);
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<DEPARTMENT_IN> GetDepartmentIn(Mrs00001Filter filter, List<V_SDA_PROVINCE> listProvince, List<V_SDA_DISTRICT> listDistrict, List<V_SDA_COMMUNE> listCommune, List<HIS_ICD> listICD)
        {

            List<DEPARTMENT_IN> result = null;
            try
            {

                string query = "--thong tin khoa dieu tri dau tien cua benh nhan\n";
                query += "select\n";
                query += "dpt.treatment_id,\n";
                query += "dpt.department_id\n";
                query += "from his_department_tran dpt\n";
                query += "join his_department_tran prev on prev.id = dpt.previous_id\n";
                query += "join (\n";

                query += string.Format("select \n");
                query += string.Format("trea.id,\n"); ;
                query += string.Format("trea.in_department_id\n");
                query += string.Format("from his_service_req sr\n");
                query += string.Format("join v_his_room ro on ro.id=sr.request_room_id\n");
                query += string.Format("join his_treatment trea on trea.id=sr.treatment_id \n");
                query += string.Format("join his_patient pt on pt.id = trea.patient_id\n");
                query += string.Format("where 1=1\n");
                query += string.Format("and sr.is_delete=0\n");
                query += string.Format("and sr.is_no_execute is null\n");
                query += string.Format("and sr.service_req_type_id={0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH);
                if (filter.INPUT_DATA_ID_TIME_TYPE == 1 || filter.INPUT_DATA_ID_TIME_TYPE == null || filter.INPUT_DATA_ID_TIME_TYPE > 7 || filter.INPUT_DATA_ID_TIME_TYPE < 1)
                {
                    FilterTime(ref query, "sr.intruction_time", filter);
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    FilterTime(ref query, "sr.start_time", filter);

                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    FilterTime(ref query, "sr.finish_time", filter);
                    query += string.Format("and sr.service_req_stt_id ={0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
                if (filter.EXAM_ROOM_ID != null)
                {
                    query += string.Format("and sr.execute_room_id ={0}\n", filter.EXAM_ROOM_ID);
                }
                if (filter.EXAM_ROOM_IDs != null)
                {
                    query += string.Format("and sr.execute_room_id in ({0})\n", string.Join(",", filter.EXAM_ROOM_IDs));
                }
                if (filter.REQUEST_ROOM_ID != null)
                {
                    query += string.Format("and sr.request_room_id = {0}\n", filter.REQUEST_ROOM_ID);
                }
                if (filter.REQUEST_ROOM_IDs != null)
                {
                    query += string.Format("and sr.request_room_id in({0})\n", string.Join(",", filter.REQUEST_ROOM_IDs));
                }
                if (filter.IS_FINISH == true)
                {
                    query += string.Format("and sr.service_req_stt_id = {0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
                query += string.Format("and (ro.room_type_id ={0} or ro.is_exam ={1})\n", IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                if (filter.PATIENT_TYPE_ID != null)
                {
                    query += string.Format("and trea.tdl_patient_type_id ={0}\n", filter.PATIENT_TYPE_ID);
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    FilterTime(ref query, "trea.in_time", filter);
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
                {
                    FilterTime(ref query, "trea.clinical_in_time", filter);
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    FilterTime(ref query, "trea.out_time", filter);
                    query += string.Format("and trea.is_pause ={0}\n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
                {
                    FilterTime(ref query, "trea.fee_lock_time", filter);
                    query += string.Format("and trea.is_active ={0}\n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                }
                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0}) \n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }
                if (filter.ICD_IDs != null)
                {
                    List<string> icd = listICD != null ? listICD.Where(p => filter.ICD_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.ICD_CODE)).Select(p => p.ICD_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.icd_code in ('{0}')\n", string.Join("','", icd));

                }

                if (filter.PROVINCE_IDs != null)
                {
                    List<string> provinceCodes = listProvince != null ? listProvince.Where(p => filter.PROVINCE_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.PROVINCE_CODE)).Select(o => o.PROVINCE_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.tdl_patient_province_code in ('{0}')\n", string.Join("','", provinceCodes));
                }
                if (filter.DISTRICT_IDs != null)
                {
                    List<string> districtCodes = listDistrict != null ? listDistrict.Where(p => filter.DISTRICT_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.DISTRICT_CODE)).Select(o => o.DISTRICT_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.tdl_patient_district_code in ('{0}')\n", string.Join("','", districtCodes));
                }
                if (filter.COMMUNE_IDs != null)
                {
                    List<string> communeCodes = listCommune != null ? listCommune.Where(p => filter.COMMUNE_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.COMMUNE_CODE)).Select(o => o.COMMUNE_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.tdl_patient_commune_code in ('{0}')\n", string.Join("','", communeCodes));
                }
                query += string.Format("and trea.in_department_id is not null\n");
                query += string.Format("group by\n");
                query += string.Format("trea.id,\n");
                query += string.Format("trea.in_department_id\n");
                query += ") trea on trea.id = dpt.treatment_id and trea.in_department_id=prev.department_id\n";
                query += "group by\n";
                query += "dpt.treatment_id,\n";
                query += "dpt.department_id\n";

                result = new MOS.DAO.Sql.SqlDAO().GetSql<DEPARTMENT_IN>(query);
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public System.Data.DataTable GetSum(Mrs00001Filter filter, string query)
        {

            System.Data.DataTable result = null;
            try
            {
                query = string.Format(query, (filter.IN_TIME_TO ?? filter.OUT_TIME_TO ?? filter.INTRUCTION_TIME_TO ?? filter.FINISH_TIME_TO ?? filter.START_TIME_TO ?? 0).ToString()
, (filter.IN_TIME_FROM ?? filter.OUT_TIME_FROM ?? filter.INTRUCTION_TIME_FROM ?? filter.FINISH_TIME_FROM ?? filter.START_TIME_FROM ?? 0).ToString()
, (filter.EXAM_ROOM_IDs != null) ? string.Join(",", filter.EXAM_ROOM_IDs) : "''"

, (filter.PATIENT_TYPE_ID != null) ? filter.PATIENT_TYPE_ID.ToString() : "''"
, (filter.TDL_PATIENT_TYPE_IDs != null) ? string.Join(",", filter.TDL_PATIENT_TYPE_IDs) : "''"
); ;
                List<string> errors = new List<string>();
                result = new MOS.DAO.Sql.SqlDAO().Execute(query, ref errors);
                Inventec.Common.Logging.LogSystem.Info(string.Join(", ", errors));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            if (ContainsSelectClause(query) && NullOrEmptyRow(result))
            {
                result = new DataTable();
                DataRow row = result.NewRow();
                result.Rows.Add(row);
            }
            return result;
        }

        private bool NullOrEmptyRow(DataTable result)
        {
            return (result == null || result.Rows == null || result.Rows.Count == 0);
        }

        private bool ContainsSelectClause(string query)
        {
            return string.IsNullOrWhiteSpace(query) == false && query.ToUpper().Contains("SELECT");
        }


        public List<SERE_SERV> GetSereServOfKCC_KKB(List<Mrs00001RDO> listRdoOfKCC_KKB, Mrs00001Filter filter, List<V_SDA_PROVINCE> listProvince, List<V_SDA_DISTRICT> listDistrict, List<V_SDA_COMMUNE> listCommune, List<HIS_ICD> listICD)
        {


            List<SERE_SERV> result = null;
            try
            {
                string query = "";
                query += string.Format("--danh sach dich vu cua benh nhan kham vao noi tru va ra khoi khoa kham benh cap cuu\n");
                query += string.Format("select \n");
                query += string.Format("ss.id,\n");
                query += "ss.service_req_id, \n";
                query += "ss.tdl_treatment_id, \n";
                query += "ss.tdl_request_room_id, \n";
                query += "ss.service_id, \n";
                query += "ss.tdl_service_type_id, \n";
                query += "ss.tdl_service_name, \n";
                query += "ss.amount \n";

                query += string.Format("from his_service_req sr\n");
                query += string.Format("join his_sere_serv ss on sr.id = ss.service_req_id\n");
                query += string.Format("join his_treatment trea on trea.id=sr.treatment_id \n");
                query += string.Format("join his_patient pt on pt.id = sr.tdl_patient_id\n");
                query += string.Format("where 1=1\n");
                query += string.Format("and ss.is_delete=0\n");
                query += string.Format("and ss.is_no_execute is null\n");
                if (filter.PATIENT_TYPE_ID != null)
                {
                    query += string.Format("and ss.patient_type_id ={0}\n", filter.PATIENT_TYPE_ID);
                }
                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and ss.patient_type_id in ({0}) \n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }

                string loopFilter = "";
                var skip = 0;
                var treatmentIds = listRdoOfKCC_KKB.Select(o => o.TREATMENT_ID).Distinct().ToList();
                while (treatmentIds.Count - skip > 0)
                {
                    var listIDs = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    loopFilter += string.Format("or ss.tdl_treatment_id in ({0})\n", string.Join(",", listIDs));
                }

                if (filter.ICD_IDs != null)
                {
                    List<string> icd = listICD != null ? listICD.Where(p => filter.ICD_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.ICD_CODE)).Select(p => p.ICD_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.icd_code in ('{0}')\n", string.Join("','", icd));

                }

                if (filter.PROVINCE_IDs != null)
                {
                    List<string> provinceCodes = listProvince != null ? listProvince.Where(p => filter.PROVINCE_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.PROVINCE_CODE)).Select(o => o.PROVINCE_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.tdl_patient_province_code in ('{0}')\n", string.Join("','", provinceCodes));
                }
                if (filter.DISTRICT_IDs != null)
                {
                    List<string> districtCodes = listDistrict != null ? listDistrict.Where(p => filter.DISTRICT_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.DISTRICT_CODE)).Select(o => o.DISTRICT_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.tdl_patient_district_code in ('{0}')\n", string.Join("','", districtCodes));
                }
                if (filter.COMMUNE_IDs != null)
                {
                    List<string> communeCodes = listCommune != null ? listCommune.Where(p => filter.COMMUNE_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.COMMUNE_CODE)).Select(o => o.COMMUNE_CODE).ToList() : new List<string>();
                    query += string.Format("and trea.tdl_patient_commune_code in ('{0}')\n", string.Join("','", communeCodes));
                }
                query += string.Format("and (1=0 {0})\n", loopFilter);
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<SERE_SERV>(query);

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
        public long Id { get; set; }
        public long? CountBhyt { get; set; }
        public long? CountVp { get; set; }
    }

    public class SALE_MEDICINE
    {
        public long TREATMENT_ID { get; set; }
        public long REQUEST_ROOM_ID { get; set; }
        public long MEDICINE_TYPE_ID { get; set; }
        public decimal AMOUNT { get; set; }

        public string MEDICINE_TYPE_NAME { get; set; }
    }

    public class DEPARTMENT_IN
    {
        public long TREATMENT_ID { get; set; }

        public long DEPARTMENT_ID { get; set; }

    }

    public class TREATMENT
    {
        public long ID { get; set; }
    }

    public class SERE_SERV
    {
        public long ID { get; set; }
        public long? SERVICE_REQ_ID { get; set; }
        public long? TDL_TREATMENT_ID { get; set; }
        public decimal AMOUNT { get; set; }
        public long SERVICE_ID { get; set; }

        public string TDL_SERVICE_NAME { get; set; }

        public long TDL_REQUEST_ROOM_ID { get; set; }

        public long TDL_SERVICE_TYPE_ID { get; set; }
    }
}

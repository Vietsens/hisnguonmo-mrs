using Inventec.Common.Logging;
using MOS.DAO.Sql;
using MOS.EFMODEL.DataModels;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00038
{
    class ManagerSql
    {
        public List<PATIENT_TREATMENT_EXAMREQ> GetMain(Mrs00038Filter filter, List<V_SDA_PROVINCE> listProvince, List<V_SDA_DISTRICT> listDistrict, List<V_SDA_COMMUNE> listCommune)
        {
            List<PATIENT_TREATMENT_EXAMREQ> result = new List<PATIENT_TREATMENT_EXAMREQ>();

          
            string query = "";
          
            query += "SELECT \n";


            query += "trea.id TREATMENT_ID, \n";

            query += "sr.EXECUTE_DEPARTMENT_ID, \n";

            query += "sr.REQUEST_ROOM_ID, \n";

            query += "sr.EXECUTE_ROOM_ID, \n";

            query += "sr.REQUEST_LOGINNAME, \n";

            query += "sr.REQUEST_USERNAME, \n";

            query += "sr.EXECUTE_LOGINNAME, \n";

            query += "sr.EXECUTE_USERNAME, \n";

            query += "sr.INTRUCTION_TIME, \n";

            query += "sr.ID, \n";

            query += "trea.ICD_CAUSE_CODE, \n";

            query += "trea.ICD_CAUSE_NAME, \n";

            query += "trea.IN_TIME, \n";

            query += "trea.CREATOR TREATMENT_CREATOR, \n";

            query += "trea.TREATMENT_END_TYPE_ID, \n";

            query += "trea.TDL_PATIENT_NAME, \n";

            query += "trea.ICD_TEXT, \n";

            query += "trea.ICD_SUB_CODE, \n";

            query += "trea.TDL_PATIENT_CAREER_NAME, \n";

            query += "trea.TDL_PATIENT_DOB, \n";

            query += "trea.TDL_PATIENT_ADDRESS, \n";

            query += "trea.tdl_patient_commune_code, \n";

            query += "trea.TDL_PATIENT_CODE, \n";

            query += "trea.TDL_PATIENT_GENDER_NAME, \n";

            query += "trea.TDL_PATIENT_PHONE, \n";

            query += "trea.TDL_PATIENT_RELATIVE_PHONE, \n";

            query += "trea.PATIENT_ID, \n";

            query += "pt.ETHNIC_NAME, \n";

            query += "pt.NATIONAL_NAME, \n";

            query += "pt.RELATIVE_TYPE, \n";

            query += "pt.RELATIVE_NAME, \n";

            query += "pt.WORK_PLACE_ID, \n";

            query += "trea.TDL_TREATMENT_TYPE_ID, \n";

            query += "trea.TREATMENT_CODE, \n";

            query += "trea.STORE_CODE, \n";

            query += "trea.IN_CODE, \n";

            query += "trea.OUT_CODE, \n";

            query += "trea.TDL_HEIN_CARD_NUMBER, \n";

            query += "trea.TDL_HEIN_MEDI_ORG_CODE, \n";

            query += "trea.TRANSFER_IN_ICD_NAME, \n";

            query += "trea.TRANSFER_IN_ICD_CODE, \n";

            query += "trea.TRANSFER_IN_MEDI_ORG_NAME, \n";

            query += "trea.TDL_PATIENT_GENDER_ID, \n";

            query += "trea.TREATMENT_RESULT_ID, \n";

            query += "trea.OUT_TIME, \n";

            query += "trea.DEATH_WITHIN_ID, \n";

            query += "trea.HOSPITALIZE_DEPARTMENT_ID, \n";

            query += "trea.LAST_DEPARTMENT_ID, \n";

            query += "trea.END_DEPARTMENT_ID, \n";

            query += "trea.END_ROOM_ID, \n";

            query += "trea.CLINICAL_IN_TIME, \n";

            query += "sr.PATIENT_CASE_ID, \n";

            query += "trea.ICD_NAME, \n";

            query += "trea.ICD_CODE, \n";

            query += "trea.IN_ICD_NAME, \n";

            query += "trea.IN_ICD_CODE, \n";

            query += "nvl(sr.IS_EMERGENCY,trea.IS_EMERGENCY) IS_EMERGENCY, \n";

            query += "trea.TREATMENT_DAY_COUNT, \n";

            query += "trea.IN_ROOM_ID, \n";

            query += "trea.IN_DEPARTMENT_ID \n";
            query += "FROM HIS_RS.HIS_TREATMENT TREA \n";
            query += "LEFT  JOIN HIS_RS.HIS_SERVICE_REQ SR ON TREA.ID = SR.TREATMENT_ID and SR.IS_DELETE =0 AND SR.IS_NO_EXECUTE IS NULL AND SR.SERVICE_REQ_TYPE_ID=1\n";
            query += "JOIN HIS_RS.HIS_PATIENT PT ON TREA.PATIENT_ID = PT.ID\n";
              

            if (filter.INPUT_DATA_ID_EXAM_TYPE == 2)//xử lý chỉ lấy công khám thu tiền
            {
                query += "JOIN HIS_RS.HIS_SERE_SERV_BILL SSB ON SSB.TDL_SERVICE_REQ_ID = SR.ID JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.ID=SSB.BILL_ID AND TRAN.IS_CANCEL IS NULL \n";
            }
            if (filter.INPUT_DATA_ID_EXAM_TYPE == 3)//xử lý chỉ lấy công khám đầu tiên
            {
                query += "LEFT JOIN HIS_RS.HIS_SERVICE_REQ SR1 ON SR1.TDL_PATIENT_ID = SR.TDL_PATIENT_ID AND SR1.ID<SR.ID\n";
            }
            query += "WHERE 1=1\n";
            if (filter.INPUT_DATA_ID_EXAM_TYPE == 3)//xử lý chỉ lấy công khám đầu tiên
            {
                query += string.Format("AND SR.id is not null and SR1.ID IS NULL \n");
            }
            if (filter.IS_CACULATION_TREATMENTT_IN == true)
            {
                query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
            }
            else if (filter.INPUT_DATA_ID_EXAM_TYPE == 2)//xử lý chỉ lấy công khám thu tiền
            {
                query += string.Format("AND TRAN.TRANSACTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
            }
            else 
            if (filter.INPUT_DATA_ID_TIME_TYPE == 8)
            {
                query += string.Format("AND SR.FINISH_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID={2}\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
            }
            else if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
            {
                query += string.Format("AND SR.START_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID<>{2}\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL);
            }
            else if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
            {
                query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
            }
            else if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
            {
                query += string.Format("AND TREA.CLINICAL_IN_TIME BETWEEN {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
            }
            else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
            {
                query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE ={2} AND TREA.TREATMENT_END_TYPE_ID = 2\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
            }
            else  if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
            {
                query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2} \n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
            }
            else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
            {
                query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE ={2}\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
            }
            else if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
            {
                query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
            }
            else
            {
                query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);

            }

            //if (filter.REQUEST_ROOM_IDs != null)
            //{
            //    query += string.Format("AND SR.REQUEST_ROOM_ID IN ({0}) \n", string.Join(",", filter.REQUEST_ROOM_IDs));
            //}

            if (filter.EXECUTE_ROOM_IDs != null)
            {
                query += string.Format("AND SR.EXECUTE_ROOM_ID IN ({0}) \n", string.Join(",", filter.EXECUTE_ROOM_IDs));
            }

            if (filter.EXAM_ROOM_IDs != null)
            {
                query += string.Format("AND SR.EXECUTE_ROOM_ID IN ({0}) \n", string.Join(",", filter.EXAM_ROOM_IDs));
            }
            if (filter.REQUEST_LOGINNAMEs != null)
            {
                query += string.Format("AND SR.REQUEST_LOGINNAME IN ('{0}') \n", string.Join("','", filter.REQUEST_LOGINNAMEs));
            }
            if (filter.EXECUTE_LOGINNAMEs != null)
            {
                query += string.Format("AND SR.EXECUTE_LOGINNAME IN ('{0}') \n", string.Join("','", filter.EXECUTE_LOGINNAMEs));
            }
          
            if (filter.PATIENT_TYPE_IDs != null)
            {
                query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID IN ({0}) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
            }
           
            if (filter.TREATMENT_TYPE_IDs != null)
            {
                query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID IN ({0}) \n", string.Join(",", filter.TREATMENT_TYPE_IDs));
            }

            if (filter.LAST_DEPARTMENT_ID != null)
            {
                query += string.Format("AND TREA.LAST_DEPARTMENT_ID ={0} \n", filter.LAST_DEPARTMENT_ID);
            }
            if (filter.LAST_DEPARTMENT_IDs != null)
            {
                query += string.Format("AND TREA.LAST_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.LAST_DEPARTMENT_IDs));
            }
            if (filter.DEPARTMENT_ID != null)
            {
                query += string.Format("AND TREA.LAST_DEPARTMENT_ID ={0} \n", filter.DEPARTMENT_ID);
            }
            if (filter.DEPARTMENT_IDs != null)
            {
                query += string.Format("AND TREA.LAST_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.DEPARTMENT_IDs));
            }

            if (filter.BRANCH_ID != null)
            {
                query += string.Format("AND TREA.BRANCH_ID ={0} \n", filter.BRANCH_ID);
            }
            if (filter.BRANCH_IDs!=null)
            {
                query += string.Format("AND TREA.BRANCH_ID IN ({0}) \n", string.Join(",", filter.BRANCH_IDs));
            }
            if (!string.IsNullOrWhiteSpace(filter.PATIENT_NAME))
            {
                query += string.Format("AND lower(pt.vir_patient_name) like '%{0}%' \n", filter.PATIENT_NAME.ToLower());
            }
            if (!string.IsNullOrWhiteSpace(filter.PATIENT_CODE))
            {
                query += string.Format("AND lower(pt.PATIENT_CODE) like '%{0}%' \n", filter.PATIENT_CODE.ToLower());
            }
            if (filter.DOB > 0)
            {
                query += string.Format("AND pt.DOB is not null and substr(pt.DOB,1,8) = substr({0},1,8) \n", filter.DOB);
            }
            if (!string.IsNullOrWhiteSpace(filter.HEIN_CARD_NUMBER))
            {
                query += string.Format("AND lower(trea.tdl_hein_card_number) like '%{0}%' \n", filter.HEIN_CARD_NUMBER.ToLower());
            }
            if (!string.IsNullOrWhiteSpace(filter.ETHNIC_NAME))
            {
                query += string.Format("AND lower(pt.ETHNIC_NAME) like '%{0}%' \n", filter.ETHNIC_NAME.ToLower());
            }
            if (!string.IsNullOrWhiteSpace(filter.NATIONAL))
            {
                query += string.Format("AND lower(pt.NATIONAL_NAME) like '%{0}%' \n", filter.NATIONAL.ToLower());
            }


            if (filter.ICD_IDs != null)
            {
                query += string.Format("AND TREA.ICD_CODE IN (select ICD_CODE from his_rs.his_icd where id in ({0})) \n", string.Join(",", filter.ICD_IDs));
            }
            if (filter.PROVINCE_IDs != null)
            {
                List<string> provinceCodes = listProvince != null ? listProvince.Where(p => filter.PROVINCE_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.PROVINCE_CODE)).Select(o => o.PROVINCE_CODE).ToList() : new List<string>();
                query += string.Format("AND TREA.TDL_PATIENT_PROVINCE_CODE IN  ('{0}') \n", string.Join("','", provinceCodes));
            }
            if (filter.DISTRICT_IDs != null)
            {
                List<string> districtCodes = listDistrict != null ? listDistrict.Where(p => filter.DISTRICT_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.DISTRICT_CODE)).Select(o => o.DISTRICT_CODE).ToList() : new List<string>();
                query += string.Format("AND TREA.TDL_PATIENT_DISTRICT_CODE IN  ('{0}') \n", string.Join("','", districtCodes));
            }
            if (filter.COMMUNE_IDs != null)
            {
                List<string> communeCodes = listCommune != null ? listCommune.Where(p => filter.COMMUNE_IDs.Contains(p.ID) && !string.IsNullOrEmpty(p.COMMUNE_CODE)).Select(o => o.COMMUNE_CODE).ToList() : new List<string>();
                query += string.Format("AND TREA.TDL_PATIENT_COMMUNE_CODE IN  ('{0}') \n", string.Join("','", communeCodes));
            }
            if (filter.PATIENT_TYPE_IDs != null)
            {
                query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID IN ({0}) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
            }
            if (filter.PATIENT_CAREER_IDs != null)
            {
                query += string.Format("AND PT.career_id IN ({0}) \n", string.Join(",", filter.PATIENT_CAREER_IDs));
            }
            if (filter.PATIENT_GENDER_IDs != null)
            {
                query += string.Format("AND trea.TDL_PATIENT_GENDER_ID IN ({0}) \n", string.Join(",", filter.PATIENT_GENDER_IDs));
            }
            if (filter.AGE_FROM != null)
            {
                query += string.Format("AND trea.TDL_PATIENT_DOB >={0} \n", filter.AGE_FROM);
            }
            if (filter.AGE_TO != null)
            {
                query += string.Format("AND trea.TDL_PATIENT_DOB <{0} \n", filter.AGE_TO);
            }
            if (filter.TREATMENT_END_TYPE_IDs != null)
            {
                query += string.Format("AND trea.TREATMENT_END_TYPE_ID IN ({0}) \n", string.Join(",", filter.TREATMENT_END_TYPE_IDs));
            }


            if (filter.CREATOR_LOGINNAME != null)
            {
                query += string.Format("AND trea.CREATOR ='{0}' \n", filter.CREATOR_LOGINNAME);
            }
            if (filter.END_DEPARTMENT_IDs != null)
            {
                query += string.Format("AND TREA.END_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.END_DEPARTMENT_IDs));
            }
            if (filter.USED_DEPARTMENT_IDs != null)
            {
                query += string.Format("and EXISTS (SELECT 1 FROM HIS_RS.HIS_DEPARTMENT_TRAN DPT where TREA.ID = DPT.TREATMENT_ID and DPT.DEPARTMENT_IN_TIME IS NOT NULL and DPT.DEPARTMENT_ID IN ({0}))\n", string.Join(",", filter.USED_DEPARTMENT_IDs));
            }
            
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
          
            {
                result = new SqlDAO().GetSql<PATIENT_TREATMENT_EXAMREQ>(query);
            }
            
            
            Inventec.Common.Logging.LogSystem.Info("Finish Query ");

            return result;
        }

    
    }
    
}

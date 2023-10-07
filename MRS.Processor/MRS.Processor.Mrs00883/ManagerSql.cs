using Inventec.Common.Logging;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace MRS.Processor.Mrs00883
{
    class ManagerSql
    {
        internal List<Mrs00883RDO> GetListExam(Mrs00883Filter filter)
        {
            List<Mrs00883RDO> result = new List<Mrs00883RDO>();
            try
            {
                string query = "select \n";
                query += "sr.intruction_date MONTH, \n";
                query += "sr.service_req_stt_id, \n";
                query += "ss.tdl_service_type_id, \n";
                query += "t.treatment_end_type_id, \n";
                query += "t.tdl_patient_dob, \n";
                query += "t.id treatment_id, \n";
                query += "t.tdl_patient_gender_name \n";
                //query += string.Format("sum(case when ss.tdl_service_type_id = {0} and sr.service_req_stt_id = {1} then 1 else 0 end) as AMOUNT_TEST, \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                //query += string.Format("sum(case when a.category_code = 'XQ' and sr.service_req_stt_id = {0} then 1 else 0 end) as AMOUNT_XQUANG, \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                //query += string.Format("sum(case when a.category_code = 'MRI' and sr.service_req_stt_id = {0} then 1 else 0 end) as AMOUNT_MRI, \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                //query += string.Format("sum(case when a.category_code = 'CT' and sr.service_req_stt_id = {0} then 1 else 0 end) as AMOUNT_CT, \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                //query += string.Format("sum(case when ss.tdl_service_type_id = {0} and sr.service_req_stt_id = {1} then 1 else 0 end) as AMOUNT_SIEUAM, \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                //query += string.Format("sum(case when t.treatment_end_type_id = {0} then 1 else 0 end) as AMOUNT_CHUYEN_TUYEN \n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN);
                query += "from his_sere_serv ss \n";
                query += "join his_service_req sr on ss.service_req_id = sr.id \n";
                //query += "join ( \n";
                //query += "select src.service_id, rtc.category_code, rtc.category_name \n";
                //query += "from his_service_rety_cat src \n";
                //query += "join his_report_type_cat rtc on src.report_type_cat_id = rtc.id \n";
                //query += "where rtc.report_type_code = 'MRS00883' \n";
                //query += ") a on ss.service_id = a.service_id \n";
                query += "join his_treatment t on ss.tdl_treatment_id = t.id \n";
                query += "where 1=1 \n";
                query += string.Format("and sr.intruction_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                query += "and sr.is_delete = 0 \n";
                LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00883RDO>(query);
            }
            catch(Exception e)
            {
                LogSystem.Error(e);
                result = null;
            }
            return result;
        }
        internal List<Mrs00883RDO> GetListOther(Mrs00883Filter filter)
        {
            List<Mrs00883RDO> result = new List<Mrs00883RDO>();
            try
            {
                string query = "select \n";
                query += "sr.intruction_date MONTH, \n";
                query += string.Format("sum(case when ss.tdl_service_type_id = {0} and sr.service_req_stt_id = {1} then 1 else 0 end) as AMOUNT_TEST, \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                query += string.Format("sum(case when a.category_code = 'XQ' and sr.service_req_stt_id = {0} then 1 else 0 end) as AMOUNT_XQUANG, \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                query += string.Format("sum(case when a.category_code = 'MRI' and sr.service_req_stt_id = {0} then 1 else 0 end) as AMOUNT_MRI, \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                query += string.Format("sum(case when a.category_code = 'CT' and sr.service_req_stt_id = {0} then 1 else 0 end) as AMOUNT_CT, \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                query += string.Format("sum(case when ss.tdl_service_type_id = {0} and sr.service_req_stt_id = {1} then 1 else 0 end) as AMOUNT_SIEUAM, \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                query += string.Format("sum(case when t.treatment_end_type_id = {0} then 1 else 0 end) as AMOUNT_CHUYEN_TUYEN \n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN);
                query += "from his_sere_serv ss \n";
                query += "join his_service_req sr on ss.service_req_id = sr.id \n";
                query += "join ( \n";
                query += "select src.service_id, rtc.category_code, rtc.category_name \n";
                query += "from his_service_rety_cat src \n";
                query += "join his_report_type_cat rtc on src.report_type_cat_id = rtc.id \n";
                query += "where rtc.report_type_code = 'MRS00883' \n";query +=
                query += ") a on ss.service_id = a.service_id \n";
                query += "join his_treatment t on ss.tdl_treatment_id = t.id \n";
                query += "where 1=1 \n";
                query += string.Format("and sr.intruction_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                query += "and sr.is_delete = 0 \n";
                query += string.Format("and sr.service_req_stt_id = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                query += "group by sr.intruction_date \n";
                LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00883RDO>(query);
            }
            catch(Exception e)
            {
                LogSystem.Error(e);
                result = null;
            }
            return result;
        }
    }
}

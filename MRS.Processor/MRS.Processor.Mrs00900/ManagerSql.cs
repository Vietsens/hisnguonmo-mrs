using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00900
{
    class ManagerSql
    {
        internal List<LIST_FOR_PROCESS> GetListData(Mrs00900Filter filter)
        {
            List<LIST_FOR_PROCESS> result = new List<LIST_FOR_PROCESS>();
            try
            {
                string query = "select \n";
                query += "ss.service_req_id, \n";
                query += "ss.service_id, \n";
                query += "ssp.pttt_catastrophe_id,\n";
                query += "sr.service_req_stt_id, \n";
                query += "ss.tdl_service_type_id, \n";
                query += "ss.tdl_treatment_id, \n";
                query += "t.tdl_patient_dob, \n";
                query += "t.tdl_patient_type_id, \n";
                query += "t.treatment_end_type_id, \n";
                query += "t.tdl_treatment_type_id, \n";
                query += "t.tran_pati_reason_id, \n";
                query += "nvl(t.tran_pati_tech_id,0) tran_pati_tech_id, \n";
                query += "t.tran_pati_form_id, \n";
                query += "t.treatment_day_count, \n";
                query += "t.treatment_result_id, \n";
                query += "ssp.death_within_id, \n"; ;
                query += "nvl(ssp.pttt_group_id,0) pttt_group_id, \n";
                query += "a.category_code, \n";
                query += "a.category_name, \n";
                query += "h.hein_approval_code \n";
                query += "from his_sere_serv ss \n";
                query += "join his_service_req sr on ss.service_req_id = sr.id \n";
                query += "join his_treatment t on ss.tdl_treatment_id = t.id \n";
                query += "join his_hein_approval h on ss.tdl_treatment_id = h.treatment_id \n";
                query += "left join his_sere_serv_pttt ssp on ss.id = ssp.sere_serv_id \n";
                query += "left join \n";
                query += "( \n";
                query += "select src.service_id, rtc.category_code, rtc.category_name\n";
                query += "from his_service_rety_cat src \n";
                query += "join his_report_type_cat rtc on src.report_type_cat_id = rtc.id \n";
                query += "where 1=1 \n";
                query += "and rtc.report_type_code = 'MRS00900' \n";
                query += ") a on a.service_id = ss.service_id\n";
                
                query += "where 1=1 \n";
                query += string.Format("and t.in_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<LIST_FOR_PROCESS>(query);
                LogSystem.Info("SQL Statement: " + query);
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                result = null;
            }
            return result;
        }

        public List<HIS_TRAN_PATI_TECH> GetHIS_TRAN_PATI_TECH()
        {
            List<HIS_TRAN_PATI_TECH> result = new List<HIS_TRAN_PATI_TECH>();
            try
            {
                string query = "select * from his_tran_pati_tech";
                LogSystem.Info("SQL Statement for HIS_TRAN_PATI_TECH: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TRAN_PATI_TECH>(query);
            }
            catch(Exception e)
            {
                LogSystem.Error(e);
                result = null;
            }
            return result;
        }

        public List<Mrs00900RDO> GetService()
        {
            List<Mrs00900RDO> result = new List<Mrs00900RDO>();
            try
            {
                string query = "select sv.id service_id, pr.id parent_service_id, pr.service_code parent_service_code, pr.service_name parent_service_name\n";
                query += "from his_service pr \n";
                query += "join his_service sv on sv.parent_id = pr.id \n";
                LogSystem.Info("SQL Statement GetService: " + result);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00900RDO>(query);
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

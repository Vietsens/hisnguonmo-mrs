using Inventec.Common.Logging;
using Inventec.Core;
using MOS.DAO.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00727
{
    internal class ManagerSql
    {
        public class DEBATE 
        {
            public long ID { get; set; }
            public long DEBATE_TIME { get; set; }
            public string DEBATE_TIME_STR { get; set; }
            public string PATIENT_CODE { get; set; }
            public string TREATMENT_CODE { get; set; }
            public string PATIENT_NAME { get; set; }
            public long REQUEST_DEPARTMENT_ID { get; set; }
            public string LOGINNAME { get; set; }
            public string USERNAME { get; set; }
            public long EXECUTE_ROLE_ID { get; set; }
            public string DEBATE_DOCTORs { get; set; }
            public long? DEBATE_TYPE_ID { get; set; }
            public string PATIENT_TYPE_NAME { get; set; }
            public string TREATMENT_TYPE_NAME { get; set; }
            public long CONTENT_TYPE { get; set; }
            public string CONTENT_TYPE_NAME { get; set; }
        }

        internal List<DEBATE> GetRdo(Mrs00727Filter filter)
        {

            List<DEBATE> list = new List<DEBATE>();
            CommonParam val = new CommonParam();
            try
            {
                string text = "--danh sach benh nhan hoi chan\n";
                text += "select \n";
                text += "db.id, \n";
                text += "db.debate_time, \n";
                text += "trea.tdl_patient_code PATIENT_CODE, \n";
                text += "trea.tdl_patient_name PATIENT_NAME, \n";
                text += "trea.treatment_code, \n";
                text += "db.department_id REQUEST_DEPARTMENT_ID, \n";
                text += "treaty.TREATMENT_TYPE_NAME, \n";
                text += "paty.PATIENT_TYPE_NAME, \n";
                text += "db.debate_type_id, \n";
                
                text += "db.content_type \n";
                text += "from his_debate db\n";
                text += "join his_treatment trea on db.treatment_id = trea.id\n";
                text += "left join his_patient_type paty on paty.Id = trea.TDL_PATIENT_TYPE_ID \n";
                text += "left join his_treatment_type treaty on treaty.id = trea.tdl_treatment_type_id \n";
                text += "where 1=1\n";
                text += string.Format("and db.debate_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);

                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    text += string.Format("and db.department_id in ({0})\n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }

                //1. khac ------ 2.thuoc ---------- 3. truoc phau thuat
                if (filter.IS_ELSE_DEBATE == true && filter.IS_MEDICAL_DEBATE == true && filter.IS_BEFORE_SURGERY_DEBATE == true)
                {
                    text += "and db.content_type in (1,2,3)\n";
                }
                else if (filter.IS_ELSE_DEBATE == true && filter.IS_MEDICAL_DEBATE == false && filter.IS_BEFORE_SURGERY_DEBATE == true)
                {
                    text += "and db.content_type in (1,3)\n";
                }
                else if (filter.IS_ELSE_DEBATE == true && filter.IS_MEDICAL_DEBATE == true && filter.IS_BEFORE_SURGERY_DEBATE == false)
                {
                    text += "and db.content_type in (1,2)\n";
                }
                else if (filter.IS_ELSE_DEBATE == false && filter.IS_MEDICAL_DEBATE == true && filter.IS_BEFORE_SURGERY_DEBATE == true)
                {
                    text += "and db.content_type in (2,3)\n";
                }
                else if (filter.IS_ELSE_DEBATE == true && filter.IS_MEDICAL_DEBATE == false && filter.IS_BEFORE_SURGERY_DEBATE == false)
                {
                    text += "and db.content_type = 1\n";
                }
                else if (filter.IS_ELSE_DEBATE == false && filter.IS_MEDICAL_DEBATE == true && filter.IS_BEFORE_SURGERY_DEBATE == false)
                {
                    text += "and db.content_type = 2\n";
                }
                else if (filter.IS_ELSE_DEBATE == false && filter.IS_MEDICAL_DEBATE == false && filter.IS_BEFORE_SURGERY_DEBATE == true)
                {
                    text += "and db.content_type = 3\n";
                }

                if (filter.DEBATE_TYPE_IDs != null)
                {
                    text += string.Format("and db.debate_type_id in ({0})\n", string.Join(",", filter.DEBATE_TYPE_IDs));
                }

                if (filter.BRANCH_IDs != null)
                {
                    text += string.Format("and TREA.BRANCH_ID in ({0})\n", string.Join(",", filter.BRANCH_IDs));
                }

                LogSystem.Info("SQL: " + text);
                list = new SqlDAO().GetSql<DEBATE>(text);
                LogSystem.Info("Result: " + list);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                list = null;
            }
            return list;
        } 
    }
}

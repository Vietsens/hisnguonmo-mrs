using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00723
{
    internal class ManagerSql
    {
        public class TREATMENT
        {
            public long TGIAN_VAO { get; set; }
            public string MA_DT { get; set; }
            public string TEN_BN { get; set; }
            public long NGAY_SINH { get; set; }
            public string DIA_CHI { get; set; }
            public string NGHE_NGHIEP { get; set; }
            public long? LOAI_BN { get; set; }
            public string SO_THE { get; set; }
            public int ID_DT { get; set; }
            public long? ID_KP { get; set; }
            public string TEN_KP { get; set; }
        }

        internal List<TREATMENT> GetDataTreatment(Mrs00723Filter filter)
        {
            List<TREATMENT> result = new List<TREATMENT>();
            try
            {
                string query = "select \n";
                query += "trea.in_time TGIAN_VAO,\n";
                query += "trea.treatment_code MA_DT,\n";
                query += "trea.tdl_patient_name TEN_BN,\n";
                query += "trea.tdl_patient_dob NGAY_SINH,\n";
                query += "trea.tdl_patient_address DIA_CHI,\n";
                query += "trea.tdl_patient_career_name NGHE_NGHIEP,\n";
                query += "trea.tdl_patient_type_id LOAI_BN,\n";
                query += "trea.tdl_hein_card_number SO_THE,\n";
                query += "trea.id ID_DT,\n";
                query += "trea.in_department_id ID_KP,\n";
                query += "dp.department_name TEN_KP\n";
                
                query += "from his_treatment trea\n";
                query += "join l_his_service_req sr on trea.id = sr.treatment_id \n";
                query += "join his_department dp on trea.in_department_id = dp.id\n";
                query += "where 1=1\n";
                query += "and sr.service_req_stt_id = 1\n";
                query += string.Format("and trea.in_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (filter.DEPARTMENT_IDs != null)
                {
                    query += string.Format("and trea.in_department_id in ({0})\n", string.Join(",", filter.DEPARTMENT_IDs));
                }
                if (filter.ROOM_IDs != null)
                {
                    query += string.Format("and trea.in_room_id in ({0})\n", string.Join(",", filter.ROOM_IDs));
                }
                query += "order by trea.in_time desc";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<TREATMENT>(query);
            
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
            return result;
        }
    }
}

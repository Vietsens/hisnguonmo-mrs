using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00722
{
    internal class ManagerSql
    {
        public class SERVICE_REQ
        {
            public string MA_BN { get; set; }
            public string TEN_BN { get; set; }
            public int GIOI_TINH { get; set; }
            public long NGAY_SINH { get; set; }
            public string DIA_CHI { get; set; }
            public long? LOAI_BN { get; set; }
            public long TGIAN_VAO { get; set; }
            public long? TGIAN_CHIDINH { get; set; }
            public long? TGIAN_THUCHIEN { get; set; }
            public long MA_KP_CHIDINH { get; set; }
            public long MA_KP_THUCHIEN { get; set; }
            public string NGUOI_CHIDINH { get; set; }
            public string NGUOI_THUCHIEN { get; set; }
            public string ICD { get; set; }
            public string CHAN_DOAN { get; set; }
            public string KET_LUAN { get; set; }
            public decimal? GIA { get; set; }
            public decimal? SO_LUONG { get; set; }
            public string MA_DV { get; set; }
            public string TEN_DV { get; set; }
            public string TEN_KP { get; set; }
        }

        internal List<SERVICE_REQ> GetDataServiceReq(Mrs00722Filter filter)
        {
            List<SERVICE_REQ> result = new List<SERVICE_REQ>();
            try
            {
                string query = "select \n";
                
                query += "trea.tdl_patient_code MA_BN,\n";
                query += "trea.tdl_patient_name TEN_BN, \n";
                query += "trea.tdl_patient_gender_id GIOI_TINH, \n";
                query += "trea.tdl_patient_dob NGAY_SINH, \n";
                query += "trea.tdl_patient_address DIA_CHI, \n";
                query += "trea.tdl_patient_type_id LOAI_BN, \n";
                query += "trea.in_time TGIAN_VAO,\n";
                query += "sr.intruction_time TGIAN_CHIDINH,\n";
                query += "ss.execute_time TGIAN_THUCHIEN,\n";
                query += "sr.request_department_id MA_KP_CHIDINH,\n";
                query += "sr.execute_department_id MA_KP_THUCHIEN,\n";
                query += "sr.request_username NGUOI_CHIDINH,\n";
                query += "sr.execute_username NGUOI_THUCHIEN,\n";
                query += "sr.icd_name ICD,\n";
                query += "sr.icd_text CHAN_DOAN,\n";
                query += "sse.conclude KET_LUAN,\n";
                query += "ss.price GIA,\n";
                query += "ss.amount SO_LUONG,\n";
                query += "ss.tdl_service_code MA_DV,\n";
                query += "ss.tdl_service_name TEN_DV,\n";
                query += "dp.department_name TEN_KP\n";
                query += "from his_sere_serv ss\n";
                query += "join his_sere_serv_ext sse on ss.id = sse.sere_serv_id\n";
                query += "join his_service_req sr on ss.service_req_id = sr.id\n";
                
                query += "join his_treatment trea on ss.tdl_treatment_id = trea.id\n";
                query += "join his_department dp on sr.request_department_id = dp.id\n";
                query += "where 1=1\n";
                query += "and sr.is_delete = 0\n";
                query += "and ss.is_delete = 0\n";
                query += "and ss.tdl_service_code is not null\n";
                query += string.Format("and sr.intruction_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
                if (filter.DEPARTMENT_ID != null)
                {
                    query += string.Format("and sr.execute_department_id = {0}\n", filter.DEPARTMENT_ID);
                }
                query += "order by ss.tdl_service_code";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<SERVICE_REQ>(query);
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

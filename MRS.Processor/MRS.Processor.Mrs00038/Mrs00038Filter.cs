using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00038
{
    public class Mrs00038Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public long? END_DEPARTMENT_ID { get; set; }
        public long? BRANCH_ID { get; set; }
        public List<long> BRANCH_IDs { get; set; }
        public short? TIME_TYPE { get; set; }
        public List<long> PROVINCE_IDs { get; set; }
        public List<long> DISTRICT_IDs { get; set; }
        public List<long> COMMUNE_IDs { get; set; }
        public List<long> ICD_IDs { get; set; }
        public long? INPUT_DATA_ID_TIME_TYPE { get; set; }//1: vào viện, 2: ra viện, 3: khóa viện phí, 4: chuyển viện, 5: nhập viện, 6: chỉ định khám, 7: bắt đầu khám, 8: kết thúc khám
        public long? AGE_FROM { get; set; }
        public long? AGE_TO { get; set; }
        public long INPUT_DATA_ID_EXAM_TYPE { get; set; }//1: số lượng đã khám toàn bv, 2: Số lượt thu tiền khám toàn bv, 3: số lượt khám đầu tiên, 4: Số lượt bệnh nhân đăng ký, 5: dành cho mẫu 1
        public long INPUT_DATA_ID_ROUTE_TYPE { get; set; }//1: tất cả, 2: đúng tuyến, 3: trái tuyến
        public List<long> EXAM_ROOM_IDs { get; set; }
        public string CREATOR_LOGINNAME { get; set; }
        public List<string> REQUEST_LOGINNAMEs { get; set; }
        public List<long> TREATMENT_END_TYPE_IDs { get; set; }
        public List<long> PATIENT_CAREER_IDs { get; set; }
        public List<long> PATIENT_GENDER_IDs { get; set; }

        
        public string PATIENT_CODE { set; get; }
        public string PATIENT_NAME {set;get;}
        public long? DOB { set; get; }
        public string HEIN_CARD_NUMBER { set; get; }
        public string ETHNIC_NAME { get; set; }
        public string NATIONAL { get; set; }
        public string REQUEST_USERNAME { get; set; }
        public string EXAM_ROOM_NAME { get; set; }


        public List<long> END_DEPARTMENT_IDs { get; set; }
        public long? LAST_DEPARTMENT_ID { get; set; }
        public List<long> LAST_DEPARTMENT_IDs { get; set; }

        public List<string> EXECUTE_LOGINNAMEs { get; set; }
        public List<long> EXECUTE_ROOM_IDs { set; get; }
        public bool IS_CACULATION_TREATMENTT_IN { set; get; }
        public List<long> USED_DEPARTMENT_IDs { get; set; }
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00347
{
    
    public class Mrs00347Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public bool? TRUE_FALSE { get; set; }//true: Thoi gian ra vien; false: thoi gian vao vien
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public List<long> END_DEPARTMENT_IDs { get; set; }
        public bool? IS_NOT_REQUIRED_DETAIL { get; set; }
        public List<long> TREATMENT_END_TYPE_IDs { get; set; }
        public bool? IS_FEE_LOCK_TIME { get; set; }
        public bool? IS_PATIENT_PRICE_ZERO { get; set; }
        public short? INPUT_DATA_ID_TIME_TYPE { get; set; } //null :ra viện | 0: vao vien | 1: Khóa vp |2: giám định bhyt

        public bool? IS_GIVE_BACK { get; set; }//chỉ lấy những bệnh nhân có số tiền trả lại bệnh nhân <0

        public bool? IS_ACTIVE { get; set; }//chỉ lấy những bệnh nhân đang mở viện phí

        public bool? IS_OWE { get; set; }//chỉ lấy những bệnh nhân đang nợ tiền
        
        public Mrs00347Filter() { }

        public List<string> CASHIER_LOGINNAMEs { get; set; }

        public List<long> BRANCH_IDs { get; set; }
    }
}

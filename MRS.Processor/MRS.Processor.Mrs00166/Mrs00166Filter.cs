using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00166
{
    public class Mrs00166Filter
    {
        public long? FEE_LOCK_TIME_FROM { get; set; }
        public long? FEE_LOCK_TIME_TO { get; set; }
        public long? OUT_TIME_FROM { get; set; }
        public long? OUT_TIME_TO { get; set; }
        public long? HEIN_LOCK_TIME_FROM { get; set; }
        public long? HEIN_LOCK_TIME_TO { get; set; }

        public long? SESE_PATIENT_TYPE_ID { get; set; }
        public long? TREAT_PATIENT_TYPE_ID { get; set; }
        public long? SERVICE_TYPE_ID { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public long? END_DEPARTMENT_ID { get; set; }
        public long? REQUEST_DEPARTMENT_ID { get; set; }
        public long? EXECUTE_DEPARTMENT_ID { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; }
        public List<long> END_DEPARTMENT_IDs { get; set; }
        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }
        public List<long> EXECUTE_DEPARTMENT_IDs { get; set; }

        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public bool? CHOOSE_TIME { get; set; } //Lọc theo: null ra viện; false: khóa viện phí; true: khóa bảo hiểm

        public bool? CHOOSE_DEPARTMENT { get; set; }
        public long? INPUT_DATA_ID_TIME_TYPE { set; get; }

        public long? IN_TIME_FROM { get; set; }
        public long? IN_TIME_TO { get; set; }
        public List<long> SERVICE_TYPE_IDs { get; set; }
        public long? SERVICE_ID { get; set; }
        public List<long> SERVICE_IDs { get; set; }
        public string SERVICE_NAME { get; set; }
        public long? EXACT_PARENT_SERVICE_ID { get; set; }
        public List<long> EXACT_PARENT_SERVICE_IDs { get; set; }
        public long? TREATMENT_TYPE_ID { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public long? SERVICE_REQ_STT_ID { get; set; }
        public List<long> SERVICE_REQ_STT_IDs { get; set; }
        public List<long> TDL_PATIENT_TYPE_IDs { get;  set; }
        public List<long> PATIENT_TYPE_IDs { get;  set; }
        public long? PATIENT_CLASSIFY_ID { get;  set; }
        public List<long> PATIENT_CLASSIFY_IDs { get;  set; }
    }
}

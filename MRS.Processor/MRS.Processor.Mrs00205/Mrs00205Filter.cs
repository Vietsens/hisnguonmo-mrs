using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00205
{
    public class Mrs00205Filter
    {
        public long? DATE_FROM { get; set; }
        public long? DATE_TO { get; set; }
        public long? TRANSACTION_TIME_FROM { get; set; }
        public long? TRANSACTION_TIME_TO { get; set; }
        public long? TREAT_PATIENT_TYPE_ID { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public List<long> SERVICE_TYPE_IDs { get; set; }
        public List<long> SERVICE_IDs { get; set; }
        public long? SERVICE_TYPE_ID { get; set; }
        public long? SERVICE_ID { get; set; }
        public bool? HAS_BILL { get; set; }
        public long? TREATMENT_RESULT_ID { set; get; }
        public List<long> TREATMENT_RESULT_IDs { set; get; }
        public bool? SPLIT_REQUSER_REQROOM { get; set; }

    }
}

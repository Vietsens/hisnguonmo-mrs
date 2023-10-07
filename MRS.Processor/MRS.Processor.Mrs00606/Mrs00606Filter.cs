using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00606
{
    public class Mrs00606Filter
    {
        public long? INTRUCTION_TIME_FROM { get; set; }
        public long? INTRUCTION_TIME_TO { get; set; }
        public long? TRANSACTION_TIME_FROM { get; set; }
        public long? TRANSACTION_TIME_TO { get; set; }
        public long? FINISH_TIME_FROM { get; set; }
        public long? FINISH_TIME_TO { get; set; }
        public bool? HAS_BILL { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
    }
}

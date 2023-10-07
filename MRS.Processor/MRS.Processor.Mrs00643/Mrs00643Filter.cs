using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00643
{
    public class Mrs00643Filter
    {
        public long INTRUCTION_TIME_FROM { get; set; }
        public long INTRUCTION_TIME_TO { get; set; }
        public bool? HAS_EXPEND { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public bool? IS_PAY { get; set; }
        public bool? IS_PAUSE { get; set; }
    }
}

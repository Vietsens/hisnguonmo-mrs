using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00538
{
    public class Mrs00538Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }
        public long? PATIENT_TYPE_ID_WITH_CARD { get; set; }
        public long? PATIENT_TYPE_ID_WITH_SERVICE { get; set; }
    }
}

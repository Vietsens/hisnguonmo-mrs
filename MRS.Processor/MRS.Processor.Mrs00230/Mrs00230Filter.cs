using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00230
{
    public class Mrs00230Filter
    {
        public long DATE_TIME_FROM { get;  set;  }

        public long DATE_TIME_TO { get; set; }

        public long? DEPARTMENT_ID { get; set; }

        public List<long> DEPARTMENT_IDs { get; set; }
        public List<long> TDL_PATIENT_TYPE_IDs { get; set; }
    }
}

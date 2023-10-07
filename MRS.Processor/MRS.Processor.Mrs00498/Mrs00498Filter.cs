using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00498
{
    public class Mrs00498Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public bool IS_CAPD { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
    }
}

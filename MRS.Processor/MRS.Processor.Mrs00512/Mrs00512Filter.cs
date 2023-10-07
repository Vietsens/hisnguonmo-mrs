using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00512
{
    public class Mrs00512Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }
        public string TEST_DEPARTMENT_CODEs { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public List<long> REPORT_TYPE_CAT_IDs { get; set; }
    }
}

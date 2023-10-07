using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00437
{
    public class Mrs00437Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }
        public bool? IS_NOT_ACCUMULATE { get; set; }
        public bool? IS_BHYT { get; set; }
    }
}

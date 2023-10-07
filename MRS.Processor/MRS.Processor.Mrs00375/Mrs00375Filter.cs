using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00375
{
    public class Mrs00375Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public List<long> REPORT_TYPE_CAT_IDs { get;  set;  }
    }
}

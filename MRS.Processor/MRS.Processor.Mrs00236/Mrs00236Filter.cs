using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00236
{
    public class Mrs00236Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public List<long> MEDI_STOCK_IDs { get;  set;  }
        public List<long> REPORT_TYPE_CAT_IDs { get;  set;  }
    }
}

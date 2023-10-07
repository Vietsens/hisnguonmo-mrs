using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00393
{
    public class Mrs00393Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public List<long> MEDI_STOCK_IDs { get;  set;  }
        public long? IMP_MEDI_STOCK_ID { get;  set;  }
    }
}

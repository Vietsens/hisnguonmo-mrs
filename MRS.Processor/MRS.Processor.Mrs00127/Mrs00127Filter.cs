using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00127
{
    class Mrs00127Filter
    {
        public List<long> MEDI_STOCK_ID { get;  set;  }

        public long DATE_FROM { get;  set;  }
        public long DATE_TO { get;  set;  }
        public List<long> EXP_MEST_TYPE_IDs { get; set; }
    }
}

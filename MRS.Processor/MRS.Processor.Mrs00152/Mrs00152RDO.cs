using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00152
{
    public class Mrs00152RDO
    {
        public string NUM_ORDER { get;  set;  }
        public string CREATE_TIME { get;  set;  }
        public string BUYER_NAME { get;  set;  }
        public string SELLER_TAX_CODE { get;  set;  }
        public decimal? VIR_TOTAL_PRICE { get;  internal set;  }
        public string NOTE { get;  set;  }
    }
}

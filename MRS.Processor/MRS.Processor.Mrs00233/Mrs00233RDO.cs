using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00233
{
    public class Mrs00233RDO
    {

        public string TIME { get;  set;  }
        public string MEST_CODE { get;  set;  }
        public string MEST_TYPE_NAME { get;  set;  }
        public Decimal PRICE { get;  set;  }
        public Decimal EXP_AMOUNT { get;  set;  }
        public Decimal EXP_TOTAL_PRICE { get;  set;  }
        public Decimal IMP_AMOUNT { get;  set;  }
        public Decimal IMP_TOTAL_PRICE { get;  set;  }
        
    }
}

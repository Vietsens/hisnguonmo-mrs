using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00429
{
    class Mrs00429RDO
    {
        public decimal PRICE { get;  set;  }
        public decimal TOTAL_PRICE_DI { get;  set;  }
        public decimal TOTAL_PRICE_CT { get;  set;  }
        public decimal AMOUNT_DI { get;  set;  }
        public decimal AMOUNT_CT { get;  set;  }
        public string GROUP_NAME { get;  set;  }
        public Mrs00429RDO() { }
        
    }
}

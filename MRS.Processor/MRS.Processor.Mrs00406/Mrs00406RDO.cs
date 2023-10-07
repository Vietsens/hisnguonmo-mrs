using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00406
{
    public class Mrs00406RDO
    {
        public string HEIN_SERVICE_BHYT_CODE { get;  set;  }
        public string SERVICE_CODE { get;  set;  }
        public string SERVICE_BHYT_NAME { get;  set;  }
        public decimal AMOUNT { get;  set;  }
        public decimal PRICE { get;  set;  }
        public decimal? TOTAL_PRICE { get;  set;  }
    }
}

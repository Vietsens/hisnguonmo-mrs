using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00149
{
    class Mrs00149RDO
    {
        public string MEDICINE_TYPE_CODE { get;  set;  }
        public string MEDICINE_TYPE_NAME { get;  set;  }
        public string ACTIVE_INGR_BHYT_CODE { get;  set;  }
        public string ACTIVE_INGR_BHYT_NAME { get;  set;  }
        public string MANUFACTURER_NAME { get;  set;  }
        public string NATIONAL_NAME { get;  set;  }
        public decimal? PRICE { get;  set;  }
        public decimal? AMOUNT_USED { get;  set;  }
        public decimal? TOTAL_PRICE { get;  set;  }
    }
}

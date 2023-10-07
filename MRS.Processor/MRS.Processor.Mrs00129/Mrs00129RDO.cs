using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00129
{
    class Mrs00129RDO
    {
        public string EXP_TIME { get;  set;  }
        public string PATIENR_CODE { get;  set;  }
        public string PATIENR_NAME { get;  set;  }
        public string CLIENT_NAME { get;  set;  }
        public string MEDICINE_TYPE_CODE { get;  set;  }
        public string MEDICINE_TYPE_NAME { get;  set;  }
        public decimal TOTAL_AMOUNT { get;  set;  }
        public decimal? TOTAL_DISCOUNT { get;  set;  }
        public decimal? TOTAL_PRICE { get;  set;  }
        public decimal? VAT_RATIO { get;  set;  }
    }
}

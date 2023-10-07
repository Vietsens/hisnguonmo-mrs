using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00471
{
    class Mrs00471RDO
    {
        public long GROUP_ID { get;  set;  }
        public string GROUP_NAME { get;  set;  }
        public string SERVICE_ID { get;  set;  }
        public string SERVICE_NAME { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }
        public decimal AMOUNT { get;  set;  }
        public decimal EXP_AMOUNT { get;  set;  }
        public decimal IMP_AMOUNT { get;  set;  }
        public decimal? PRICE_BEFORE_VAT { get;  set;  }
        public decimal? VAT { get;  set;  }
        public decimal? PRICE_AFTER_VAT { get;  set;  }
        public decimal? TOTAL_PRICE { get;  set;  }
        public Mrs00471RDO() { }
    }
}

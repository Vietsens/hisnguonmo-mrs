using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 

namespace MRS.Processor.Mrs00251
{
    public class Mrs00251RDO
    {
        public string TEMPLATE_CODE { get;  set;  }
        public string SYMBOL_CODE { get;  set;  }
        public Decimal TOTAL { get;  set;  }
        public string BEGIN_VIR_NUM_ORDER_FROM { get;  set;  }
        public string BEGIN_VIR_NUM_ORDER_TO { get;  set;  }
        public string BUY_VIR_NUM_ORDER_FROM { get;  set;  }
        public string BUY_VIR_NUM_ORDER_TO { get;  set;  }
        public string TOTAL_USERED_VIR_NUM_ORDER_FROM { get;  set;  }
        public string TOTAL_USERED_VIR_NUM_ORDER_TO { get;  set;  }
        public Decimal TOTAL_USERED_COUNT { get;  set;  }
        public Decimal USERED_COUNT { get;  set;  }
        public Decimal DEL_COUNT { get;  set;  }
        public string DEL_VIR_NUM_ORDERs { get;  set;  }
        public Decimal LOS_COUNT { get;  set;  }
        public string LOS_VIR_NUM_ORDERs { get;  set;  }
        public Decimal CAN_COUNT { get;  set;  }
        public string CAN_VIR_NUM_ORDERs { get;  set;  }
        public Decimal END_NUM { get;  set;  }
        public string END_VIR_NUM_ORDER_FROM { get;  set;  }
        public string END_VIR_NUM_ORDER_TO { get;  set;  }
        
    }
}

using MOS.EFMODEL.DataModels; 
using System; 

namespace MRS.Processor.Mrs00158
{
    class Mrs00158RDO
    {
        public string STOCK_CODE { get;  set;  }
        public string IMP_MEST_TIME { get;  set;  }
        public string IMP_MEST_CODE { get;  set;  }
        public string EXP_MEST_CODE { get;  set;  }
        public string MEDICINE_TYPE_CODE { get;  set;  }
        public string MEDICINE_TYPE_NAME { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }
        public decimal AMOUNT { get;  set;  }
        public decimal MOBA { get;  set;  }
        public decimal PRICE { get;  set;  }
        public decimal VAT { get;  set;  }
    }
}

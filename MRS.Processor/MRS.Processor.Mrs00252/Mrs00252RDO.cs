using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 

namespace MRS.Processor.Mrs00252
{
    public class Mrs00252RDO
    {
       
        public long ID { get;  set;  }
        public const string MEDICINE = "THUỐC"; 
        public const string MATERIAL = "VẬT TƯ"; 
        public string TYPE { get;  set;  }
        public string TYPE_CODE { get;  set;  }
        public string TYPE_NAME { get;  set;  }
        public string MANUFACTURER_NAME { get;  set;  }

        public string EXP_TIME_STR { get;  set;  }
        public string EXP_MEST_CODE { get;  set;  }
        public string PACKAGE_NUMBER { get;  set;  }
        public string SUPPLIER_CODE { get;  set;  }
        public string SUPPLIER_NAME { get;  set;  }
        public string BID_NUMBER { get;  set;  }
        public string EXPIRED_DATE_STR { get;  set;  }
        public Decimal IMP_PRICE { get;  set;  }
        public Decimal EXP_PRICE { get;  set;  }
        public Decimal INTERNAL_PRICE { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }
        public Decimal EXP_AMOUNT { get;  set;  }
        public Decimal MOBA_AMOUNT { get;  set;  }
        public Decimal VAT_RATIO { get;  set;  }
    }
}

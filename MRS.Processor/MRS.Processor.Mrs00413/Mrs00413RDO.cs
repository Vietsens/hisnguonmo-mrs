using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00413
{
    public class Mrs00413RDO
    {
        public string PATIENT_CODE { get;  set;  }
        public string PATIENT_NAME { get;  set;  }
        public string DEPARTMENT { get;  set;  }
        public long BILL_NUMBER { get;  set;  }
        public string SERVICE_TYPE_NAME { get;  set;  }
        public string SERVICE_CONCRETE_NAME { get;  set;  }
        public string SERVICE_NAME { get;  set;  }
        public string SERVICE_CODE { get;  set;  }
        public string IN_TIME { get;  set;  }
        public string BILL_CREATE_TIME { get;  set;  }
        public string CASHOUT_TIME { get;  set;  }
        public decimal AMOUNT { get;  set;  }
        public decimal PRICE { get;  set;  }
        public decimal? TOTAL_MONEY { get;  set;  }
        public decimal? PATIENT_PRICE { get;  set;  }
        public decimal? HEIN_PRICE { get;  set;  }
        public string USER_NAME { get;  set;  }
       
    }
}

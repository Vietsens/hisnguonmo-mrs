using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00411
{
    public class Mrs00411RDO
    {

        public string BILL_CREATE_TIME { get;  set;  }
        public string CASHOUT_TIME { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string PATIENT_NAME { get;  set;  }
        public long BILL_NUMBER { get;  set;  }
        public long INVOICE_NUMBER { get;  set;  }
        public decimal AMOUNT { get;  set;  }
        public decimal PRICE { get;  set;  }
        public decimal? TOTAL_MONEY { get;  set;  }
        public decimal? PATIENT_PRICE { get;  set;  }
        public decimal? HEIN_PRICE { get;  set;  }
        public string DEPARTMENT { get;  set;  }
        public string SERVICE_NAME { get;  set;  }
       
    }
}

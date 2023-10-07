using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00052
{
    class Mrs00052RDO
    {
        public string ACCOUNT_BOOK_CODE { get;  set;  }
        public string ACCOUNT_BOOK_NAME { get;  set;  }
        public string DESCRIPTION { get;  set;  }
        public string CREATOR { get;  set;  }

        public decimal TOTAL_TRANSACTION { get;  set;  }
        public decimal TOTAL_TRANSACTION_CANCEL { get;  set;  }
        public decimal TOTAL_COST_PRICE { get;  set;  }
        public decimal VIR_TOTAL_PATIENT_PRICE { get;  set;  }
    }
}

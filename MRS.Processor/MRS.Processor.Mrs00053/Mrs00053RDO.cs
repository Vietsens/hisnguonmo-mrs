using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00053
{
    class Mrs00053RDO
    {
        public long CREATE_TIME { get;  set;  }
        public string CREATE_DATE_STR { get;  set;  }
        public string TRANSACTION_CODE { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string VIR_PATIENT_NAME { get;  set;  }
        public string DESCRIPTION { get;  set;  }
        public decimal AMOUNT { get;  set;  }
    }
}

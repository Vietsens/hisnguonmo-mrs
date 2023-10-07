using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00025
{
    class Mrs00025RDO
    {
        public string PATIENT_TYPE_CODE { get;  set;  }
        public string PATIENT_TYPE_NAME { get;  set;  }

        public decimal VIR_TOTAL_PATIENT_PRICE { get;  set;  }
        public decimal VIR_TOTAL_HEIN_PRICE { get;  set;  }
        public decimal SERVICE_AMOUNT { get;  set;  }
    }
}

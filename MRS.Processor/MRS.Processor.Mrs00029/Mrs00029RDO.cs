using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00029
{
    class Mrs00029RDO
    {
        public string REQUEST_DEPARTMENT_CODE { get;  set;  }
        public string REQUEST_DEPARTMENT_NAME { get;  set;  }

        public decimal VIR_TOTAL_PATIENT_PRICE { get;  set;  }
        public decimal VIR_TOTAL_HEIN_PRICE { get;  set;  }
    }
}

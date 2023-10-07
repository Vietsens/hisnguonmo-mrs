using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00047
{
    class Mrs00047RDO
    {
        public string PATIENT_CODE { get;  set;  }
        public string PATIENT_NAME { get;  set;  }

        public decimal VIR_TOTAL_PRICE { get;  set;  }
        public decimal TOTAL_PAID { get;  set;  }
        public decimal TOTAL_EXEMPTION { get;  set;  }

        public decimal TOTAL_NOITRU { get;  set;  }
        public decimal TOTAL_NGOAITRU { get;  set;  }
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00070
{
    class Mrs00070RDO
    {
        public long PATIENT_TYPE_ID { get;  set;  }
        public string PATIENT_TYPE_CODE { get;  set;  }
        public string PATIENT_TYPE_NAME { get;  set;  }

        public decimal IN_VIR_TOTAL_PATIENT_PRICE { get;  set;  }
        public decimal IN_VIR_TOTAL_HEIN_PRICE { get;  set;  }

        public decimal OUT_VIR_TOTAL_PATIENT_PRICE { get;  set;  }
        public decimal OUT_VIR_TOTAL_HEIN_PRICE { get;  set;  }
    }
}

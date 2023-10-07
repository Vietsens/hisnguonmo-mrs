using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00182
{
    class VSarReportMrs00182RDO
    {
        public long TREATMENT_ID { get;  set;  }
        public string TREATMENT_CODE { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string VIR_PATIENT_NAME { get;  set;  }

        public decimal TOTAL_DEPOSIT_AMOUNT { get;  set;  }
        public decimal TOTAL_WITHDRAW_AMOUNT { get;  set;  }
    }
}

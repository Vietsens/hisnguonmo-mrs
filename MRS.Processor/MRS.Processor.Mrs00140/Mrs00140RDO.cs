using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00140
{
    class Mrs00140RDO
    {
        public long HEIN_APPROVAL_ID { get;  set;  }
        public long SERVICE_ID { get;  set;  }

        public string HEIN_APPROVAL_CODE { get;  set;  }
        public string ACTIVE_INGREDIENT_CODE { get;  set;  }
        public string SERVICE_REPORT_CODE { get;  set;  }
        public string HEIN_SERVICE_BHYT_NAME { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }
        public string CONCENTRA { get;  set;  }
        public string MEDICINE_USER_FROM_NAME { get;  set;  }
        public decimal DOSE_AMOUNT { get;  set;  }
        public string HEIN_SERVICE_BHYT_CODE { get;  set;  }
        public decimal AMOUNT { get;  set;  }
        public decimal VIR_PRICE { get;  set;  }
        public decimal HEIN_RATIO { get;  set;  }
        public decimal VIR_TOTAL_HEIN_PRICE { get;  set;  }
        public string DEPARTMENT_BHYT_CODE { get;  set;  }
        public string DOCTOR_CODE { get;  set;  }
        public string ICD_CODEs { get;  set;  }
        public string INSTRUCTION_DATE { get;  set;  }
        public short PTTT_CODE { get; set; }
        public decimal BHYT_PAY_RATE { get; set; }

        public long INSTRUCTION_TIME { get;  set;  }
    }
}

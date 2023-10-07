using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00453
{
    public class Mrs00453RDO
    {
        public string SERVICE_TYPE_NAME { get;  set;  }
        public string DEPARTMENT_NAME { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string PATIENT_NAME { get;  set;  }
        public string TREATMENT_CODE { get;  set;  }
        public long DOB { get;  set;  }
        public string GENDER_NAME { get;  set;  }
        public string IN_ICD10 { get;  set;  }
        public string OUT_ICD10 { get;  set;  }

        public long IN_TIME { get;  set;  }
        public long OUT_TIME { get;  set;  }
        public long TREATMENT_TIME { get;  set;  }
        public string BILL_TYPE_NAME { get;  set;  }
        public decimal AMOUT_INTRUCTION { get;  set;  }
        public decimal AMOUT_EXECUTE { get;  set;  }
        public decimal AMOUT_RESULT { get;  set;  }
        public decimal AMOUT_NO_RESULT { get;  set;  }
        public decimal AMOUT_NO_EXECUTE { get;  set;  }

        public decimal PRICE { get;  set;  }
        public decimal TOTAL_PRICE { get;  set;  }

        public long SERVICE_TYPE_ID { get;  set;  }
        public long DEPARTMENT_ID { get;  set;  }
        public long SERVICE_ID { get;  set;  }
        public long TREATMENT_ID { get;  set;  }

        public string SERVICE_CODE { get;  set;  }
        public string SERVICE_NAME { get;  set;  }
        public string SERVICE_TYPE_CODE { get;  set;  }

        public string DEPARTMENT_CODE { get;  set;  }
    }
}

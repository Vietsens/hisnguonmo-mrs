using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00454
{
    class Mrs00454RDO
    {
        public long DEPARTMENT_ID { get;  set;  } 
        public string DEPARTMENT_NAME { get;  set;  } 
        public string PATIENT_CODE { get;  set;  }
        public string PATIENT_NAME { get;  set;  } 
        public string TREATMENT_CODE { get;  set;  } 
        public string BIRTH_DAY { get;  set;  } 
        public string GENDER_NAME { get;  set;  } 
        public string ICD_10_IN { get;  set;  }
        public string ICD_10_OUT { get;  set;  }
        public string IN_TIME { get;  set;  }
        public string OUT_TIME { get;  set;  }
        public string PATIENT_TYPE_NAME { get;  set;  }
        public decimal AMOUNT { get;  set;  } 
        public decimal PRICE { get;  set;  } 
        public decimal AMOUNT_TT { get;  set;  }
        public decimal TOTAL_RICE { get;  set;  }
        public decimal EXEM { get;  set;  }
        public long GROUP_ID { get;  set;  }
        public string GROUP_NAME { get;  set;  }
        public string SERVICE_NAME { get;  set;  }
        public long SERVICE_ID { get;  set;  }
        public Mrs00454RDO() { }
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00403
{
    public class Mrs00403RDO
    {

        public string TREATMENT_CODE { get;  set;  }
        public string PATIENT_NAME { get;  set;  }
        public long AGE { get;  set;  }
        public string GENDER { get;  set;  }
        public string ADDRESS { get;  set;  }
        public string HEIN_CARD_NUMBER { get;  set;  }
        public string IN_TIME { get;  set;  }
        public string OUT_TIME { get;  set;  }
        public string DEPARTMENT_EXAM { get;  set;  }
        public string NOT_OUT { get;  set;  }
        public string NOT_LOCK_FEE { get;  set;  }
        public string NOT_GET_MEDICINE { get;  set;  }
       
    }
}

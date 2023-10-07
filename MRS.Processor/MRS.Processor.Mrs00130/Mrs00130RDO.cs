using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00130
{
    class Mrs00130RDO
    {
        public string TREATMENT_CODE { get;  set;  }
        public string PATIENT_NAME { get;  set;  }
        public string PATIENT_GENDER { get;  set;  }
        public long PATIENT_AGE { get;  set;  }
        public string PATIENT_ADRESS { get;  set;  }
        public string PATIENT_TYPE { get;  set;  }
        public string ICD_NAME { get;  set;  }
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00317
{
    class Mrs00317RDO
    {
        public int NUMBER { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string TREATMENT_CODE { get;  set;  }
        public string PATIENT_NAME { get;  set;  }
        public decimal? RESIDUAL { get;  set;  }
        public decimal? OWE { get;  set;  }
        public string DEPARTMENT_NAME { get;  set;  }

        public Mrs00317RDO() { }    
    }
}

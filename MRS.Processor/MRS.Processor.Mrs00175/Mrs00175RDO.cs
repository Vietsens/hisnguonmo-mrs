using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00175
{
    public class Mrs00175RDO
    {
        public long TREATMENT_ID { get;  set;  }
        public long NUMBER { get;  set;  }        
        public string PATIENT_CODE { get;  set;  }
        public string PATIENT_NAME { get;  set;  }
        public string MATERIAL_CODE { get;  set;  }
        public string MATERIAL_NAME { get;  set;  }
        public decimal? HEIN { get;  set;  }
        public decimal? FEE { get;  set;  }
        public decimal? EXPEND { get;  set;  }
    }
}

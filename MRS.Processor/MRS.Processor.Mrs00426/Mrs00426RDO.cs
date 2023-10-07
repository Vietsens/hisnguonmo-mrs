using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00426
{
    public class Mrs00426RDO
    {
        public string SERVICE_TYPE_NAME { get;  set;  }
        public decimal? TOTAL_AMOUNT { get;  set;  }
        public string SERVICE_TYPE_CODE { get;  set;  }
        //public string NATIONAL_NAME { get;  set;  }
        public decimal? PRICE { get;  set;  }
        public decimal? TOTAL_PRICE { get;  set;  }
        public string INTRUCTION_TIME { get;  set;  }
        public string GROUP_MANE { get;  set;  }
        public string PATIENT_NAME { get;  set;  }
        public string TREATMENT_CODE { get;  set;  }
    }
}

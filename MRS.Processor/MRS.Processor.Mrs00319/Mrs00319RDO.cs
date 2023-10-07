using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00319
{
    public class Mrs00319RDO
    {
        public string VIR_PATIENT_NAME { get;  set;  }
        public long DOB { get;  set;  }
        public string DOB_STR { get;  set;  }
        public long AGE { get;  set;  }
        public string GENDER_NAME { get;  set;  }
        public string VIR_ADDRESS { get;  set;  }
        public string HEIN_CARD_NUMBER { get;  set;  }
        public string OUT_TIME_STR { get;  set;  }
        public string FEE_LOCK_TIME_STR { get;  set;  }
        public string END_DEPARTMENT_NAME { get;  set;  }
        public string TREATMENT_CODE { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string STORE_CODE { get;  set;  }
    }
}

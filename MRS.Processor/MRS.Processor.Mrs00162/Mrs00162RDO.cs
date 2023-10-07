using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 

namespace MRS.Processor.Mrs00162
{
    public class Mrs00162RDO
    {
        public long TREATMENT_TYPE_ID { get;  set;  }
        public string TREATMENT_TYPE_CODE { get;  set;  }
        public string TREATMENT_TYPE_NAME { get;  set;  }

        public long TREATMENT_ID { get;  set;  }
        public string TREATMENT_CODE { get;  set;  }
        public string HEIN_CARD_NUMBER { get;  set;  }

        public string PATIENT_NAME { get;  set;  }
        public string VIR_ADDRESS { get;  set;  }
        public string IS_BHYT { get;  set;  }
        public string ICD_ENDO { get;  set;  }
        public string REQUEST_ROOM { get;  set;  }
        public string SERVICE_NAME { get;  set;  }
        public string ENDO_RESULT { get;  set;  }
        public string EXECUTE_USERNAME { get;  set;  }
        public string NOTE { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string FEMALE_YEAR { get;  set;  }
        public string MALE_YEAR { get;  set;  }

        public decimal? MALE_AGE { get;  set;  }
        public decimal? FEMALE_AGE { get;  set;  }
    }
}

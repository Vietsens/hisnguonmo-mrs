using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00423
{
    public class Mrs00423RDO
    {
        public string VIR_PATIENT_NAME { get;  set;  }
        public string DOB_YEAR_MALE { get;  set;  }
        public string DOB_YEAR_FEMALE { get;  set;  }	
        public string HEIN_CARD_NUMBER { get;  set;  }	
        public string IN_TIME_STR { get;  set;  }
        public string OUT_TIME_STR { get;  set;  }	
        public string ICD_NAME { get;  set;  }	
        public string DOCTOR_USERNAME { get;  set;  }
        public string DOCTOR_LOGINNAME { get;  set;  }

        public string END_DEPARTMENT_NAME { get;  set;  }
        public long TREAT_DAY { get;  set;  }
        public Mrs00423RDO() { }
    }
}

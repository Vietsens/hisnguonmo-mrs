using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00011
{
    class Mrs00011RDO
    {
        public string TREATMENT_CODE { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string VIR_PATIENT_NAME { get;  set;  } //V_HIS_HEIN_APPROVAL
        public string GENDER_NAME { get;  set;  }
        public string DOB { get;  set;  }
        public string VIR_ADDRESS { get;  set;  }
        public Decimal DEPOSIT_AMOUNT { get;  set;  }
        public string HEIN_CARD_NUM { get;  set;  }
        public string PATIENT_TYPE_NAME { get;  set;  }
        public string OPEN_TIME { get;  set;  }
        public string ICD_MAIN_TEXT { get;  set;  }
        public string ICD_CODE { get;  set;  }

        //Thời gian vào viên (Thời gian đầu tiên của DEPARTMENT_TRANS -> LOG_TIME)
    }
}

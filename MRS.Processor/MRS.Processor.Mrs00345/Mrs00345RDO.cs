using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00345
{
    public class Mrs00345RDO
    {
        public string PATIENT_CODE { set;  get;  }
        public string PATIENT_NAME { set;  get;  }
        public string TREATMENT_CODE { get;  set;  }
        public long DOB { set;  get;  }
        public string GENDER_NAME { get;  set;  }
        public string ADDRESS { get;  set;  }
        public string EXECUTE_ROOM_NAME { get;  set;  }
        public string IS_EXECUTE { get;  set;  }
        public string IN_PROCESS { get;  set;  }
        public string PATIENT_TYPE_NAME { get;  set;  }
        public string HEIN_CARD_NUMBER { get;  set;  }
        public long HEIN_CARD_FROM_TIME { get;  set;  }
        public long HEIN_CARD_TO_TIME { get;  set;  }
        public string ICD_NAME { get; set; }
        public string ICD_CODE { get; set; }
        public string ICD_TEXT { get; set; }
        public string ICD_SUB_CODE { get; set; }
        public string HOSPITALIZATION_REASON { set; get; }
        public string EXECUTE_USERNAME { set; get; }
        

        
    }
}

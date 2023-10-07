using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00493
{
    public class Mrs00493RDO
    {
        public long ROOM_ID { get;  set;  }
        public string ROOM_NAME { get;  set;  }

        public string PATIENT_CODE { get;  set;  }
        public string PATIENT_NAME { get;  set;  }
        public string TREATMENT_CODE { get;  set;  }
        public V_HIS_TREATMENT TREATMENT { get;  set;  }
        public V_HIS_TREATMENT_BED_ROOM TREATMENT_BED_ROOM { get;  set;  }
        public long ADD_TIME { get;  set;  }
        public long? REMOVE_TIME { get;  set;  }

        public long DOB { get;  set;  }
        public string ADDRESS { get;  set;  }
        public string GENDER_NAME { get;  set;  }
        public string HEIN_CARD_NUMBER { get;  set;  }
        public long HEIN_CARD_FROM_TIME { get;  set;  }
        public long HEIN_CARD_TO_TIME { get;  set;  }

        public string BED_NAME { get;  set;  }

        public string PHONE_NUMBER { get; set; }
        public string BED_TYPE_CODE { get; set; }
        public string BED_TYPE_NAME { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }
    }
}

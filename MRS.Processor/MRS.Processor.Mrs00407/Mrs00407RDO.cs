using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00407
{
    public class Mrs00407RDO
    {
        public string TREATMENT_CODE { get;  set;  }
        public long TREATMENT_ID { get;  set;  }
        public string PATIENT_NAME { get;  set;  }
        public long DOB { get;  set;  }
        public string GENDER_NAME { get;  set;  }
        public string ADDRESS { get;  set;  }
        public string HEIN_CARD_NUMBER { get;  set;  }
        public long IN_TIME { get;  set;  }
        public long? OUT_TIME { get; set; }
        public string EXECUTE_ROOM_NAME { get; set; }

        public Mrs00407RDO() { }
    }
}

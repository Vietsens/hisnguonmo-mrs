using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 
using System.Reflection; 

namespace MRS.Processor.Mrs00401
{
    public class Mrs00401RDO
    {
        public long TREATMENT_ID { get;  set;  }
        public string TREATMENT_CODE { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string VIR_PATIENT_NAME { get;  set;  }
        public string IS_BHYT { get;  set;  }// có phải là bhyt 
        public string VIR_ADDRESS { get;  set;  }
        public string HEIN_CARD_NUMBER { get;  set;  }
        public string GENDER_NAME { get;  set;  }
        public string CREATE_TIME_TREATMENT { get;  set;  }
        public string BILL_DATE_STR { get;  set;  }
        public int? AGE_STR { get;  set;  }
        public string EXECUTE_ROOM_NAME { get; set; }
        public long IN_TIME { get; set; }
        public long? OUT_TIME { get; set; }
        public decimal TOTAL_HEIN_PRICE { get; set; }
        public decimal TOTAL_PATIENT_PRICE { get; set; }
        public decimal TOTAL_PRICE { get; set; }
    }
}

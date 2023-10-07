using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00084
{
    class Mrs00084RDO
    {
        public long TREATMENT_ID { get;  set;  }

        public string TREATMENT_CODE { get;  set;  }
        public string HEIN_CARD_NUMBER { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string PATIENT_NAME { get;  set;  }
        public string VIR_ADDRESS { get;  set;  }
        public string SERVICE_CODE { get;  set;  }
        public string SERVICE_NAME { get;  set;  }
        public string EXECUTE_ROOM_NAME { get;  set;  }
        public string EXECUTE_USERNAME { get;  set;  }

        public decimal VIR_TOTAL_PATIENT_PRICE { get;  set;  }
        public decimal VIR_TOTAL_HEIN_PRICE { get;  set;  }

        public decimal AMOUNT { get;  set;  }
        public int? MALE_AGE { get;  set;  }
        public int? FEMALE_AGE { get;  set;  }
        public string FEMALE_YEAR { get;  set;  }
        public string MALE_YEAR { get;  set;  }
    }
}

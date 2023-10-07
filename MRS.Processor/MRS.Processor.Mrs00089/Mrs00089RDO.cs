using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00089
{
    class Mrs00089RDO
    {
        /// <summary>
        /// Theo bệnh nhân
        /// </summary>
        public long TREATMENT_ID { get;  set;  }
        public string TREATMENT_CODE { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string VIR_PATIENT_NAME { get;  set;  }
        public string VIR_ADDRESS { get;  set;  }
        public string MALE_YEAR { get;  set;  }
        public string FEMALE_YEAR { get;  set;  }
        public string HEIN_CARD_NUMBER { get;  set;  }
        public string EXAM_DATE_STR { get;  set;  }
        public string ICD_NAME { get;  set;  }

        public decimal VIR_TOTAL_PRICE { get;  set;  }
        public decimal? MALE_AGE { get;  set;  }
        public decimal? FEMALE_AGE { get;  set;  }
        public string END_ROOM_NAME { get; set; }
    }
}

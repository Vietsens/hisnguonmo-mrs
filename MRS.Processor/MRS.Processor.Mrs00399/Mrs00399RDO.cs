using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 
using System.Reflection; 

namespace MRS.Processor.Mrs00399
{
    public class Mrs00399RDO
    {
        public long TREATMENT_ID { get;  set;  }
        public string TREATMENT_CODE { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string VIR_PATIENT_NAME { get;  set;  }
        public string IS_BHYT { get;  set;  }// có phải là bhyt 
        public string VIR_ADDRESS { get;  set;  }
        public string HEIN_CARD_NUMBER { get;  set;  }
        public string GENDER_NAME { get;  set;  }
        public decimal? DAY1 { get;  set;  }
        public decimal? DAY2 { get;  set;  }
        public decimal? DAY3 { get;  set;  }
        public decimal? DAY4 { get;  set;  }
        public decimal? DAY5 { get;  set;  }
        public decimal? DAY6 { get;  set;  }
        public decimal? DAY7 { get;  set;  }
        public decimal? DAY8 { get;  set;  }
        public decimal? DAY9 { get;  set;  }
        public decimal? DAY10 { get;  set;  }
        public decimal? DAY11 { get;  set;  }
        public decimal? DAY12 { get;  set;  }
        public decimal? DAY13 { get;  set;  }
        public decimal? DAY14 { get;  set;  }
        public decimal? DAY15 { get;  set;  }
        public decimal? DAY16 { get;  set;  }
        public decimal? DAY17 { get;  set;  }
        public decimal? DAY18 { get;  set;  }
        public decimal? DAY19 { get;  set;  }
        public decimal? DAY20 { get;  set;  }
        public decimal? DAY21 { get;  set;  }
        public decimal? DAY22 { get;  set;  }
        public decimal? DAY23 { get;  set;  }
        public decimal? DAY24 { get;  set;  }
        public decimal? DAY25 { get;  set;  }
        public decimal? DAY26 { get;  set;  }
        public decimal? DAY27 { get;  set;  }
        public decimal? DAY28 { get;  set;  }
        public decimal? DAY29 { get;  set;  }
        public decimal? DAY30 { get;  set;  }
        public decimal? DAY31 { get;  set;  }
        public int? AGE_STR { get;  set;  }
        public decimal VIR_TOTAL_PRICE { get;  set;  }
        public decimal VIR_TOTAL_PATIENT_PRICE { get;  set;  }
        public decimal VIR_TOTAL_HEIN_PRICE { get;  set;  }
    }
}

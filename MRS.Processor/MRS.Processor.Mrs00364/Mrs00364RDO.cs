using MOS.EFMODEL.DataModels; 
using System;

namespace MRS.Processor.Mrs00364
{
    public class Mrs00364RDO
    {
        public string EXP_MEST_CODE { get;  set;  }
        public string BLOOD_ABO_NAME { get;  set;  }
        public Decimal AMOUNT{get; set; }
        public Decimal PRICE{get; set; }
        public Decimal TOTAL_PRICE { get;  set;  }
        public long PATIENT_ID { get;  set;  }
        public string BLOOD_TYPE_NAME { set; get; }
  
        
    }
}

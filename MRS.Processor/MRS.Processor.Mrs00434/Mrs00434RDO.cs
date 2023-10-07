using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00434
{
    class Mrs00434RDO
    {
        //public decimal PRICE { get;  set;  }
        public string GROUP_NAME { get;  set;  }
        public decimal AMOUNT_DI { get;  set;  }
        public decimal VIR_TOTAL_PATIENT_PRICE_DI { get;  set;  }
        public decimal VIR_TOTAL_HEIN_PRICE_DI { get;  set;  }
        public decimal TOTAL_PRICE_DI { get;  set;  }
        public decimal AMOUNT_CT { get;  set;  }
        public decimal VIR_TOTAL_PATIENT_PRICE_CT { get;  set;  }
        public decimal VIR_TOTAL_HEIN_PRICE_CT { get;  set;  }
        public decimal TOTAL_PRICE_CT { get; set; }
        public long SERE_SERV_ID { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public string SERVICE_NAME { get; set; }


        public Mrs00434RDO() { }
        
    }
}

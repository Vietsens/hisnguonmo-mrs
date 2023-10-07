using MOS.EFMODEL.DataModels; 
using MOS.MANAGER.HisTreatment; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00272
{
    public class Mrs00272RDO
    {
        public string SERVICE_NAME { get;  set;  }
        public decimal PRICE { get;  set;  }
        public decimal AMOUNT { get;  set;  }
        public decimal VAT_RATIO { get;  set;  }
        public decimal VIR_TOTAL_PRICE { get;  set;  }
        public long HEIN_SERVICE_TYPE_NUM_ORDER { get;  set;  }
        public long SERVICE_TYPE_ID { get;  set;  }
        public string SERVICE_TYPE_NAME { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
    }
    public class SereServSDO : HIS_SERE_SERV
    {
        public long TDL_PATIENT_DOB { get; set; }
        //public long TREATMENT_TYPE_ID { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public string SERVICE_REQ_CODE { get; set; }
        public string SERVICE_TYPE_CODE { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        //public string SERVICE_CODE { get; set; }
        //public string SERVICE_NAME { get; set; }
        //public long TDL_SERVICE_TYPE_ID { get; set; }
        
    }
}

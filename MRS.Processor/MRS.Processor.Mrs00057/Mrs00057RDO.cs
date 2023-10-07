using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00057
{
    class Mrs00057RDO : V_HIS_SERE_SERV
    {
        public Mrs00057RDO secondRDO { get; set; }
        public long TDL_TREATMENT_TYPE_ID { get; set; }
        public long PATIENT_TYPE_ID { get; set; }
        public string PATIENT_TYPE_CODE { get;  set;  }
        public string PATIENT_TYPE_NAME { get; set; }
        public string TEST_SERVICE_TYPE_CODE { get; set; }
        public string TEST_SERVICE_TYPE_NAME { get; set; }
        public string TEST_PARENT_CODE { get; set; }
        public string TEST_PARENT_NAME { get; set; }

        public decimal AMOUNT { get; set; }

        public decimal TOTAL_PRICE { get; set; }
        public decimal TOTAL_PATIENT_PRICE { get; set; }
        public decimal COST_PRICE { get;  set;  }
        public decimal FEE_PRICE { get;  set;  }

        public decimal AMOUNT_FEE { get; set; }

        public decimal AMOUNT_BHYT { get; set; }

        public decimal AMOUNT_NOITRU { get; set; }

        public decimal AMOUNT_NGOAITRU { get; set; }
    }
    
}

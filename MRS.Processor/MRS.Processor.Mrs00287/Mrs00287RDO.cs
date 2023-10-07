using MOS.EFMODEL.DataModels; 
using MRS.MANAGER.Config; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00287
{
    public class Mrs00287RDO
    {
        public long PAY_FORM_ID { get; set; }
        public string PAY_FORM_CODE { get; set; }
        public string PAY_FORM_NAME { get; set; }
        public long AREA_ID { get; set; }
        public string AREA_CODE { get; set; }
        public string AREA_NAME { get; set; }
        public string CASHIER_LOGINNAME { get; set; }
        public string CASHIER_USERNAME { get; set; }
        public decimal DEP_BIL { get; set; }
        public decimal DEP_BIL_TRANSFER { get; set; }
        public decimal REPAYs { get;  set;  }
        public decimal PATIENT_BHYT_PRICE { get; set; }
        public decimal CHENH_LECH { get; set; }
        public long TOTAL_PATIENT_COUNT { get; set; }
        public short? IS_CANCEL { get; set; }
        
        public Mrs00287RDO()
        {

        }


        public decimal EXAM_PRICE { get; set; }

        public decimal CLS_PRICE { get; set; }

        public long NUM_ORDER { get; set; }

        public string TRANS_REQ_CODE { get; set; }

        public string BANK_TRANSACTION_CODE { get; set; }

        public long? BANK_TRANSACTION_TIME { get; set; }

        public string TDL_TREATMENT_CODE { get; set; }

        public string BANK_CARD_CODE { get; set; }

        public string TDL_PATIENT_NAME { get; set; }

        public decimal BILLs { get; set; }

        public decimal DEPOSITs { get; set; }

        public string TYPE_BANK_CARD { get; set; }
    }

    public class CARD
    {
        public string BANK_CARD_CODE { get; set; }
    }
}

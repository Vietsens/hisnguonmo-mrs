using MOS.EFMODEL.DataModels; 
using MRS.MANAGER.Config; 
using MOS.MANAGER.HisTreatment; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00281
{
    public class Mrs00281RDO
    {
        public string TRANSACTION_TIME_STR { get;  set;  }
        public string TREATMENT_CODE { get;  set;  }
        public string VIR_PATIENT_NAME { get;  set;  }
        public string DEPOSIT_USERNAME { get;  set;  }
        public string REPAY_USERNAME { get;  set;  }
        public Decimal DEPOSIT_AMOUNT { get;  set;  }
        public Decimal REPAY_AMOUNT { get;  set;  }
        public string PAY_FORM_NAME { get;  set;  }




        public long CASHIER_ROOM_ID { get; set; }

        public string CASHIER_ROOM_CODE { get; set; }

        public string CASHIER_ROOM_NAME { get; set; }

        public short? IS_CANCEL { get; set; }

        public string REPAY_LOGINNAME { get; set; }

        public decimal DEPOSIT_CANCEL_AMOUNT { get; set; }

        public decimal BILL_AMOUNT { get; set; }

        public string DEPOSIT_LOGINNAME { get; set; }

        public decimal BILL_CANCEL_AMOUNT { get; set; }

        public string CASHIER_USERNAME { get; set; }

        public string CASHIER_LOGINNAME { get; set; }

        public decimal? KC_AMOUNT { get; set; }

        public string PAY_FORM_CODE { get; set; }

        public long PAY_FORM_ID { get; set; }

        public string VIR_PATIENT_CODE { get; set; }

        public decimal? EXTRA_AMOUNT { get; set; }
    }
}

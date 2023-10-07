using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00094
{
    class Mrs00094RDO
    {
        public MOS.EFMODEL.DataModels.V_HIS_TRANSACTION Bill { get; set; }
        public string TRANSACTION_CODE { get; set; }
        public string PATIENT_CODE { get;  set;  }
        public string CANCEL_USERNAME { get;  set;  }
        public string CANCEL_REASON { get;  set;  }
        public string VIR_PATIENT_NAME { get;  set;  }
        public string CREATE_TIME_STR { get;  set;  }
        public decimal AMOUNT { get;  set;  }

        public Mrs00094RDO(MOS.EFMODEL.DataModels.V_HIS_TRANSACTION bill)
        {
            Bill = bill ?? new MOS.EFMODEL.DataModels.V_HIS_TRANSACTION();
            TRANSACTION_CODE = bill.TRANSACTION_CODE; 
            PATIENT_CODE = bill.TDL_PATIENT_CODE; 
            CANCEL_USERNAME = bill.CANCEL_USERNAME; 
            CANCEL_REASON = bill.CANCEL_REASON; 
            VIR_PATIENT_NAME = bill.TDL_PATIENT_NAME; 
            AMOUNT = bill.AMOUNT;
            CREATE_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(bill.TRANSACTION_TIME); 
        }
    }
}

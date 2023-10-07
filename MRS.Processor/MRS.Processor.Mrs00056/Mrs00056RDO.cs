using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00056
{
    class Mrs00056RDO
    {
        public long CREATE_TIME { get;  set;  }
        public string CREATE_DATE_STR { get;  set;  }
        public string TRANSACTION_CODE { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string VIR_PATIENT_NAME { get;  set;  }
        public string DESCRIPTION { get;  set;  }
        public decimal AMOUNT { get;  set;  }

        public Mrs00056RDO(HIS_TRANSACTION Bill)
        {
            try
            {
                TRANSACTION_CODE = Bill.TRANSACTION_CODE; 
                PATIENT_CODE = Bill.TDL_PATIENT_CODE; 
                VIR_PATIENT_NAME = Bill.TDL_PATIENT_NAME; 
                DESCRIPTION = Bill.DESCRIPTION; 
                AMOUNT = Bill.AMOUNT;
                CREATE_TIME = Bill.TRANSACTION_TIME;
                CREATE_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(Bill.TRANSACTION_TIME); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}

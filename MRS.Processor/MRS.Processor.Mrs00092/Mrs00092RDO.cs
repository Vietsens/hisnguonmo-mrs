using MOS.MANAGER.HisTreatment;
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00092
{
    class Mrs00092RDO
    {
        public string CREATE_DATE_STR { get;  set;  }
        public string TRANSACTION_CODE { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string VIR_PATIENT_NAME { get;  set;  }
        public string DESCRIPTION { get;  set;  }
        public string CASHIER_USERNAME { get;  set;  }
        public string IS_OUT { get;  set;  }
        public long NUM_ORDER { get;  set;  }

        public decimal AMOUNT { get;  set;  }
        public string ACCOUNT_BOOK_CODE { get;  set;  }
        public string ACCOUNT_BOOK_NAME { get;  set;  }
        public string CANCEL_LOGINNAME { get;  set;  }
        public string CANCEL_REASON { get;  set;  }
        public long? CANCEL_TIME { get;  set;  }
        public string CASHIER_LOGINNAME { get;  set;  }
        public string CASHIER_ROOM_CODE { get;  set;  }
        public string CASHIER_ROOM_NAME { get;  set;  }
        public long? CREATE_TIME { get;  set;  }
        public string CREATOR { get;  set;  }
        public string DEPOSIT_REQ_CODE { get;  set;  }
        public long DOB { get;  set;  }
        public string GENDER_CODE { get;  set;  }
        public string GENDER_NAME { get;  set;  }
        public string GROUP_CODE { get;  set;  }
        public string MODIFIER { get;  set;  }
        public long? MODIFY_TIME { get;  set;  }
        public string PAY_FORM_CODE { get;  set;  }
        public string PAY_FORM_NAME { get;  set;  }
        public long TRANSACTION_TIME { get;  set;  }
        public string TREATMENT_CODE { get;  set;  }
        public string VIR_ADDRESS { get;  set;  }

        public Mrs00092RDO(MOS.EFMODEL.DataModels.V_HIS_TRANSACTION Deposit)
        {
            try
            {
                TRANSACTION_CODE = Deposit.TRANSACTION_CODE; 
                PATIENT_CODE = Deposit.TDL_PATIENT_CODE; 
                VIR_PATIENT_NAME = Deposit.TDL_PATIENT_NAME; 
                DESCRIPTION = Deposit.DESCRIPTION; 
                AMOUNT = Deposit.AMOUNT; 
                CASHIER_USERNAME = Deposit.CASHIER_USERNAME; 
                NUM_ORDER = Deposit.NUM_ORDER; 
                ACCOUNT_BOOK_CODE = Deposit.ACCOUNT_BOOK_CODE; 
                ACCOUNT_BOOK_NAME = Deposit.ACCOUNT_BOOK_NAME; 
                CANCEL_LOGINNAME = Deposit.CANCEL_LOGINNAME; 
                CANCEL_REASON = Deposit.CANCEL_REASON; 
                CANCEL_TIME = Deposit.CANCEL_TIME; 
                CASHIER_LOGINNAME = Deposit.CASHIER_LOGINNAME; 
                CASHIER_ROOM_CODE = Deposit.CASHIER_ROOM_CODE; 
                CASHIER_ROOM_NAME = Deposit.CASHIER_ROOM_NAME; 
                CASHIER_USERNAME = Deposit.CASHIER_USERNAME; 
                CREATE_TIME = Deposit.CREATE_TIME; 
                CREATOR = Deposit.CREATOR; 
                DEPOSIT_REQ_CODE = Deposit.TRANSACTION_CODE; 
                DOB = Deposit.TDL_PATIENT_DOB ?? 0; 
                GENDER_NAME = Deposit.TDL_PATIENT_GENDER_NAME; 
                GROUP_CODE = Deposit.GROUP_CODE; 
                MODIFIER = Deposit.MODIFIER; 
                MODIFY_TIME = Deposit.MODIFY_TIME; 
                PAY_FORM_CODE = Deposit.PAY_FORM_CODE; 
                PAY_FORM_NAME = Deposit.PAY_FORM_NAME; 
                TRANSACTION_TIME = Deposit.TRANSACTION_TIME; 
                TREATMENT_CODE = Deposit.TREATMENT_CODE; 
                VIR_ADDRESS = Deposit.TDL_PATIENT_ADDRESS; 
                CREATE_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(Deposit.CREATE_TIME ?? 0); 
                ProcessIsOutTreatment(Deposit); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void ProcessIsOutTreatment(MOS.EFMODEL.DataModels.V_HIS_TRANSACTION deposit)
        {
            try
            {
                var treatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager().GetViewById(deposit.TREATMENT_ID ?? 0); 
                if (treatment != null)
                {
                    if (treatment.TREATMENT_END_TYPE_ID > 0)
                    {
                        IS_OUT = "Đã ra"; 
                    }
                    else
                    {
                        IS_OUT = "Chưa ra"; 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}

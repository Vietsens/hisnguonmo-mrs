using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00515
{
    class Mrs00515RDO
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
        
        
        public string VIR_ADDRESS{ get;  set;  }						
        public string HEIN_CARD_NUMBER{ get;  set;  }					
        public string DEPARTMENT_NAME{ get;  set;  }


        public Mrs00515RDO(HIS_TRANSACTION Deposit, Dictionary<long, HIS_DEPARTMENT_TRAN> dicDepartmentTran, List<V_HIS_TREATMENT_FEE> listTreatmentFee, List<HIS_PAY_FORM> ListHisPayForm, List<HIS_ACCOUNT_BOOK> ListHisAccountBook, List<HIS_CASHIER_ROOM> ListHisCashierRoom)
        {
            try
            {
                if (dicDepartmentTran != null)
                {
                    if (dicDepartmentTran.ContainsKey(Deposit.ID))
                    {
                        DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == dicDepartmentTran[Deposit.ID].DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                    }
                }
                if (listTreatmentFee != null)
                {
                    var treatmentFee = listTreatmentFee.FirstOrDefault(o => o.ID == Deposit.TREATMENT_ID) ?? new V_HIS_TREATMENT_FEE();
                    if (treatmentFee.TREATMENT_END_TYPE_ID > 0)
                    {
                        IS_OUT = "Đã ra";
                    }
                    else
                    {
                        IS_OUT = "Chưa ra";
                    }
                    VIR_ADDRESS = treatmentFee.TDL_PATIENT_ADDRESS;
                    HEIN_CARD_NUMBER = treatmentFee.TDL_HEIN_CARD_NUMBER;
                }

                TRANSACTION_CODE = Deposit.TRANSACTION_CODE; 
                PATIENT_CODE = Deposit.TDL_PATIENT_CODE; 
                VIR_PATIENT_NAME = Deposit.TDL_PATIENT_NAME; 
                DESCRIPTION = Deposit.DESCRIPTION; 
                AMOUNT = Deposit.AMOUNT;
                CASHIER_USERNAME = Deposit.CASHIER_USERNAME;
                CASHIER_LOGINNAME = Deposit.CASHIER_LOGINNAME;
                NUM_ORDER = Deposit.NUM_ORDER;
                var accountBook = ListHisAccountBook.FirstOrDefault(o => o.ID == Deposit.ACCOUNT_BOOK_ID);
                if (accountBook != null)
                {
                    ACCOUNT_BOOK_CODE = accountBook.ACCOUNT_BOOK_CODE;
                    ACCOUNT_BOOK_NAME = accountBook.ACCOUNT_BOOK_NAME;
                }
                CANCEL_LOGINNAME = Deposit.CANCEL_LOGINNAME; 
                CANCEL_REASON = Deposit.CANCEL_REASON; 
                CANCEL_TIME = Deposit.CANCEL_TIME; 
                var cashierRoom = ListHisCashierRoom.FirstOrDefault(o => o.ID == Deposit.CASHIER_ROOM_ID);
                if (cashierRoom != null)
                {
                    CASHIER_ROOM_CODE = cashierRoom.CASHIER_ROOM_CODE;
                    CASHIER_ROOM_NAME = cashierRoom.CASHIER_ROOM_NAME;
                }
                CREATE_TIME = Deposit.CREATE_TIME; 
                CREATOR = Deposit.CREATOR; 
                DEPOSIT_REQ_CODE = Deposit.TRANSACTION_CODE; 
                DOB = Deposit.TDL_PATIENT_DOB ?? 0; 
                GENDER_NAME = Deposit.TDL_PATIENT_GENDER_NAME; 
                GROUP_CODE = Deposit.GROUP_CODE; 
                MODIFIER = Deposit.MODIFIER;
                MODIFY_TIME = Deposit.MODIFY_TIME;
                var payForm = ListHisPayForm.FirstOrDefault(o => o.ID == Deposit.PAY_FORM_ID);
                if (payForm != null)
                {
                    PAY_FORM_CODE = payForm.PAY_FORM_CODE;
                    PAY_FORM_NAME = payForm.PAY_FORM_NAME;
                }
                TRANSACTION_TIME = Deposit.TRANSACTION_TIME; 
                TREATMENT_CODE = Deposit.TDL_TREATMENT_CODE; 
                VIR_ADDRESS = Deposit.TDL_PATIENT_ADDRESS; 
                CREATE_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(Deposit.CREATE_TIME ?? 0); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        public Mrs00515RDO()
        {
            // TODO: Complete member initialization
        }
    }
}

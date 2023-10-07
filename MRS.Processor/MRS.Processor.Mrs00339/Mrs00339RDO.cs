using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00339
{
    class Mrs00339RDO
    {
        public Mrs00339RDO(V_HIS_TRANSACTION data, decimal totalFund)
        {
            if (data != null)
            {
                this.ACCOUNT_BOOK_CODE = data.ACCOUNT_BOOK_CODE; 
                this.ACCOUNT_BOOK_NAME = data.ACCOUNT_BOOK_NAME; 
                this.CASHIER_LOGINNAME = data.CASHIER_LOGINNAME; 
                this.CASHIER_ROOM_CODE = data.CASHIER_ROOM_CODE; 
                this.CASHIER_ROOM_NAME = data.CASHIER_ROOM_NAME; 
                this.CASHIER_USERNAME = data.CASHIER_USERNAME;
                this.CREATE_TIME = data.TRANSACTION_TIME;
                this.CREATE_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(data.TRANSACTION_TIME); 
                this.DOB = data.TDL_PATIENT_DOB ?? 0; 
                this.EXEMPTION = data.EXEMPTION; 
                this.KC_AMOUNT = data.KC_AMOUNT; 
                this.PATIENT_ID = data.TDL_PATIENT_ID ?? 0; 
                this.PATIENT_CODE = data.TDL_PATIENT_CODE; 
                this.VIR_PATIENT_NAME = data.TDL_PATIENT_NAME; 
                this.GENDER_NAME = data.TDL_PATIENT_GENDER_NAME; 

                if (data.PAY_FORM_ID == IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__CK)
                {
                    this.TOTAL_BILL_CK_AMOUNT = data.AMOUNT; 
                }

                this.TOTAL_BILL_FUND_AMOUNT = totalFund; 

                this.TOTAL_DEPOSIT_BILL_AMOUNT = data.AMOUNT - (data.KC_AMOUNT ?? 0) - (data.EXEMPTION ?? 0) - totalFund; 
            }
        }

        public Mrs00339RDO(V_HIS_TRANSACTION data)
        {
            if (data != null)
            {
                this.CASHIER_LOGINNAME = data.CASHIER_LOGINNAME; 
                this.CASHIER_USERNAME = data.CASHIER_USERNAME;
                this.CREATE_TIME = data.TRANSACTION_TIME;
                this.CREATE_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(data.TRANSACTION_TIME); 
                this.DOB = data.TDL_PATIENT_DOB??0; 
                this.PATIENT_ID = data.TDL_PATIENT_ID ?? 0; 
                this.PATIENT_CODE = data.TDL_PATIENT_CODE;  ; 
                this.VIR_PATIENT_NAME = data.TDL_PATIENT_NAME; 

                this.GENDER_NAME = data.TDL_PATIENT_GENDER_NAME; 

                if (data.PAY_FORM_ID == IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__CK)
                {
                    this.TOTAL_BILL_CK_AMOUNT = data.AMOUNT; 
                }
                if(data.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU)
                this.TOTAL_DEPOSIT_BILL_AMOUNT = data.AMOUNT; 
                else if (data.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU)
                this.TOTAL_REPAY_AMOUNT = data.AMOUNT; 
            }
        }

      
        public Mrs00339RDO(List<Mrs00339RDO> item)
        {
            this.ACCOUNT_BOOK_CODE = item.First().ACCOUNT_BOOK_CODE; 
            this.ACCOUNT_BOOK_NAME = item.First().ACCOUNT_BOOK_NAME; 
            this.CASHIER_LOGINNAME = item.First().CASHIER_LOGINNAME; 
            this.CASHIER_ROOM_CODE = item.First().CASHIER_ROOM_CODE; 
            this.CASHIER_ROOM_NAME = item.First().CASHIER_ROOM_NAME; 
            this.CASHIER_USERNAME = item.First().CASHIER_USERNAME; 
            this.CREATE_TIME = item.First().CREATE_TIME; 
            this.CREATE_TIME_STR = item.First().CREATE_TIME_STR; 
            this.DOB = item.First().DOB; 
            this.EXEMPTION = item.Sum(o => o.EXEMPTION); 
            this.KC_AMOUNT = item.Sum(o => o.KC_AMOUNT); 
            this.PATIENT_ID = item.First().PATIENT_ID; 
            this.PATIENT_CODE = item.First().PATIENT_CODE;  ; 
            this.VIR_PATIENT_NAME = item.First().VIR_PATIENT_NAME; 
            this.GENDER_NAME = item.First().GENDER_NAME; 

            this.TOTAL_DEPOSIT_BILL_AMOUNT = item.Sum(o => o.TOTAL_DEPOSIT_BILL_AMOUNT); 

            this.TOTAL_REPAY_AMOUNT = item.Sum(o => o.TOTAL_REPAY_AMOUNT); 

            this.TOTAL_BILL_FUND_AMOUNT = item.Sum(o => o.TOTAL_BILL_FUND_AMOUNT); 

            this.TOTAL_BILL_CK_AMOUNT = item.Sum(o => o.TOTAL_BILL_CK_AMOUNT); 
        }

        public Mrs00339RDO() { }

        public long PATIENT_ID { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string VIR_PATIENT_NAME { get;  set;  }
        public string GENDER_NAME { get;  set;  }
        public long DOB { get;  set;  }

        public long? CREATE_TIME { get;  set;  }
        public string CREATE_TIME_STR { get;  set;  }

        public string CASHIER_LOGINNAME { get;  set;  }
        public string CASHIER_USERNAME { get;  set;  }
        public string CASHIER_ROOM_CODE { get;  set;  }
        public string CASHIER_ROOM_NAME { get;  set;  }
        public string ACCOUNT_BOOK_CODE { get;  set;  }
        public string ACCOUNT_BOOK_NAME { get;  set;  }

        public decimal? EXEMPTION { get;  set;  }
        public decimal? KC_AMOUNT { get;  set;  }

        public decimal? TOTAL_DEPOSIT_BILL_AMOUNT { get;  set;  }
        public decimal? TOTAL_REPAY_AMOUNT { get;  set;  }
        public decimal? TOTAL_BILL_FUND_AMOUNT { get;  set;  }
        public decimal? TOTAL_BILL_CK_AMOUNT { get;  set;  }
    }
}

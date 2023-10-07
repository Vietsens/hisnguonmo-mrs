using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00631
{
    class Mrs00631RDO
    {
        public long PATIENT_TYPE_ID { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }

        public long TREATMENT_ID { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string PATIENT_CODE { get; set; }
        public string VIR_PATIENT_NAME { get; set; }
        public string GENDER_NAME { get; set; }
        public long DOB { get; set; }

        public string CASHIER_LOGINNAME { get; set; }
        public string CASHIER_USERNAME { get; set; }
        public string CASHIER_ROOM_CODE { get; set; }
        public string CASHIER_ROOM_NAME { get; set; }

        public long ACCOUNT_BOOK_ID { get; set; }
        public string ACCOUNT_BOOK_CODE { get; set; }
        public string ACCOUNT_BOOK_NAME { get; set; }

        public decimal? EXEMPTION { get; set; }
        public decimal? KC_AMOUNT { get; set; }

        public long? CREATE_TIME { get; set; }
        public string CREATE_TIME_STR { get; set; }
        public long? DEPOSIT_BILL_NUM_ORDER { get; set; }
        public long? REPAY_NUM_ORDER { get; set; }
        public string DEPOSIT_BILL_TRANSACTION_CODE { get; set; }
        public string REPAY_TRANSACTION_CODE { get; set; }
        public string TRANSACTION_TYPE_NAME_PLUS { get; set; }
        public decimal? TOTAL_DEPOSIT_BILL_AMOUNT { get; set; }
        public decimal? TOTAL_REPAY_AMOUNT { get; set; }

        public Mrs00631RDO() { }

        public Mrs00631RDO(V_HIS_TRANSACTION data, decimal totalFund, V_HIS_PATIENT_TYPE_ALTER patyAlter)
        {
            try
            {
                if (data != null && patyAlter != null)
                {
                    this.ACCOUNT_BOOK_CODE = data.ACCOUNT_BOOK_CODE;
                    this.ACCOUNT_BOOK_ID = data.ACCOUNT_BOOK_ID;
                    this.ACCOUNT_BOOK_NAME = data.ACCOUNT_BOOK_NAME;
                    this.CASHIER_LOGINNAME = data.CASHIER_LOGINNAME;
                    this.CASHIER_ROOM_CODE = data.CASHIER_ROOM_CODE;
                    this.CASHIER_ROOM_NAME = data.CASHIER_ROOM_NAME;
                    this.CASHIER_USERNAME = data.CASHIER_USERNAME;
                    this.CREATE_TIME = data.TRANSACTION_TIME;
                    this.CREATE_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(data.TRANSACTION_TIME);
                    this.DOB = data.TDL_PATIENT_DOB ?? 0;
                    this.EXEMPTION = data.EXEMPTION;
                    this.KC_AMOUNT = data.KC_AMOUNT;
                    this.PATIENT_CODE = data.TDL_PATIENT_CODE;
                    this.DEPOSIT_BILL_NUM_ORDER = data.NUM_ORDER;
                    this.TREATMENT_ID = data.TREATMENT_ID ?? 0;
                    this.VIR_PATIENT_NAME = data.TDL_PATIENT_NAME;
                    this.TREATMENT_CODE = data.TREATMENT_CODE;

                    this.PATIENT_TYPE_ID = patyAlter.PATIENT_TYPE_ID;
                    this.PATIENT_TYPE_NAME = patyAlter.PATIENT_TYPE_NAME;

                    if (data.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT)
                    {
                        this.DEPOSIT_BILL_TRANSACTION_CODE = data.TRANSACTION_CODE;
                        this.TRANSACTION_TYPE_NAME_PLUS = "Thanh toán";
                        this.TOTAL_DEPOSIT_BILL_AMOUNT = data.AMOUNT - (data.KC_AMOUNT ?? 0) - (data.EXEMPTION ?? 0) - totalFund;
                    }
                    else if (data.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU)
                    {
                        this.DEPOSIT_BILL_TRANSACTION_CODE = data.TRANSACTION_CODE;
                        this.TRANSACTION_TYPE_NAME_PLUS = "Tạm ứng dịch vụ";
                        this.TOTAL_DEPOSIT_BILL_AMOUNT = data.AMOUNT;
                    }
                    else if (data.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU)
                    {
                        this.REPAY_TRANSACTION_CODE = data.TRANSACTION_CODE;
                        this.TRANSACTION_TYPE_NAME_PLUS = "Hoàn ứng dịch vụ";
                        this.TOTAL_REPAY_AMOUNT = data.AMOUNT;
                    }
                    else
                    {
                        this.DEPOSIT_BILL_TRANSACTION_CODE = data.TRANSACTION_CODE;
                        this.TRANSACTION_TYPE_NAME_PLUS = data.TRANSACTION_TYPE_NAME;
                        this.TOTAL_DEPOSIT_BILL_AMOUNT = data.AMOUNT;
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

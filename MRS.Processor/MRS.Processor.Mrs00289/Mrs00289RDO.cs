using MOS.EFMODEL.DataModels;
using MRS.Processor.Mrs00296;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00289
{
    public class Mrs00289RDO
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

        public string PRE_CASHIER_LOGINNAME { get; set; }
        public string PRE_CASHIER_USERNAME { get; set; }

        public long CASHIER_ROOM_ID { get; set; }

        public string CASHIER_ROOM_CODE { get; set; }
        public string CASHIER_ROOM_NAME { get; set; }

        public long ACCOUNT_BOOK_ID { get; set; }
        public string ACCOUNT_BOOK_CODE { get; set; }
        public string ACCOUNT_BOOK_NAME { get; set; }
        public long PAY_FORM_ID { get; set; }
        public string PAY_FORM_CODE { get; set; }
        public string PAY_FORM_NAME { get; set; }
        public string BANK_CARD_CODE { get; set; }

        public decimal? EXEMPTION { get; set; }
        public decimal? KC_AMOUNT { get; set; }

        public long? CREATE_TIME { get; set; }
        public long TRANSACTION_DATE { get; set; }
        public string CREATE_TIME_STR { get; set; }
        public long? DEPOSIT_BILL_NUM_ORDER { get; set; }
        public long? REPAY_NUM_ORDER { get; set; }
        public string DEPOSIT_BILL_TRANSACTION_CODE { get; set; }
        public string REPAY_TRANSACTION_CODE { get; set; }
        public string TRANSACTION_TYPE_NAME_PLUS { get; set; }
        public decimal? TOTAL_DEPOSIT_BILL_AMOUNT { get; set; }
        public decimal? TOTAL_REPAY_AMOUNT { get; set; }
        public string TRANSACTION_TYPE_CODE { get; set; }
        public string TRANSACTION_CODE { get; set; }
        public string TIG_TRANSACTION_CODE { get; set; }

        public short? IS_CANCEL { get; set; }
        public long AREA_ID { get; set; }
        public string AREA_CODE { get; set; }
        public string AREA_NAME { get; set; }
        public long SS_PATIENT_TYPE_ID { get; set; }
        public string SS_PATIENT_TYPE_CODE { get; set; }
        public string SS_PATIENT_TYPE_NAME { get; set; }
        //them tien benh nhan tu tra va benh nhan dong chi tra
        public decimal PATIENT_BHYT_PRICE { get; set; }
        public decimal CHENH_LECH { get; set; }

        public short? IS_ACTIVE { get; set; }

        public Mrs00289RDO() { }

        public Mrs00289RDO(V_HIS_TRANSACTION data, decimal totalFund, V_HIS_PATIENT_TYPE_ALTER patyAlter, Mrs00289Filter filter, Mrs00296RDO ss)
        {
            if (data != null)
            {
                this.IS_ACTIVE = data.IS_ACTIVE;
                this.ACCOUNT_BOOK_CODE = data.ACCOUNT_BOOK_CODE;
                this.ACCOUNT_BOOK_ID = data.ACCOUNT_BOOK_ID;
                this.ACCOUNT_BOOK_NAME = data.ACCOUNT_BOOK_NAME;
                //this.ACCOUNT_BOOK_CODE = data.ACCOUNT_BOOK_CODE;
                this.PAY_FORM_ID = data.PAY_FORM_ID;
                this.PAY_FORM_NAME = data.PAY_FORM_NAME;
                this.PAY_FORM_CODE = data.PAY_FORM_CODE;
                this.BANK_CARD_CODE = data.TDL_BANK_CARD_CODE;
                this.TRANSACTION_CODE = data.TRANSACTION_CODE;
                this.TIG_TRANSACTION_CODE = data.TIG_TRANSACTION_CODE;
                this.CASHIER_LOGINNAME = data.CASHIER_LOGINNAME;
                if (filter.IS_HIDE_CASHIER_ROOM != true)
                {
                    this.CASHIER_ROOM_ID = data.CASHIER_ROOM_ID;
                    this.CASHIER_ROOM_CODE = data.CASHIER_ROOM_CODE;
                    this.CASHIER_ROOM_NAME = data.CASHIER_ROOM_NAME;
                }
                this.CASHIER_USERNAME = data.CASHIER_USERNAME;
                this.PRE_CASHIER_LOGINNAME = data.CASHIER_LOGINNAME;
                this.PRE_CASHIER_USERNAME = data.CASHIER_USERNAME;
                this.CREATE_TIME = data.TRANSACTION_TIME;
                this.TRANSACTION_DATE = data.TRANSACTION_DATE;
                this.CREATE_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(data.TRANSACTION_TIME);
                this.DOB = data.TDL_PATIENT_DOB ?? 0;
                this.EXEMPTION = data.EXEMPTION;
                this.KC_AMOUNT = data.KC_AMOUNT;
                this.PATIENT_CODE = data.TDL_PATIENT_CODE;
                this.DEPOSIT_BILL_TRANSACTION_CODE = data.TRANSACTION_CODE;
                this.TRANSACTION_TYPE_NAME_PLUS = "Thanh toán";
                this.TREATMENT_ID = data.TREATMENT_ID ?? 0;
                this.VIR_PATIENT_NAME = data.TDL_PATIENT_NAME;
                this.TREATMENT_CODE = data.TREATMENT_CODE;
                this.TRANSACTION_TYPE_CODE = data.TRANSACTION_TYPE_CODE;
                this.IS_CANCEL = data.IS_CANCEL;
                if (patyAlter != null)
                {
                    this.PATIENT_TYPE_ID = patyAlter.PATIENT_TYPE_ID;
                    this.PATIENT_TYPE_NAME = patyAlter.PATIENT_TYPE_NAME;
                }
                if (ss != null)
                {
                    this.TOTAL_DEPOSIT_BILL_AMOUNT = ss.TOTAL_DEPOSIT_BILL_PRICE;
                    if (filter.IS_ADD_BILL_CANCEL == true && data.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        this.TOTAL_REPAY_AMOUNT = ss.TOTAL_REPAY_PRICE;
                    }
                    this.PATIENT_BHYT_PRICE = ss.TOTAL_PATIENT_BHYT_PRICE;
                    this.CHENH_LECH = ss.TOTAL_DEPOSIT_BILL_PRICE - ss.TOTAL_PATIENT_BHYT_PRICE;
                    if (data.AMOUNT > 0)
                    {
                        this.DEP_BIL_TRANSFER = ss.TOTAL_DEPOSIT_BILL_PRICE * (data.TRANSFER_AMOUNT ?? 0) / data.AMOUNT;
                    }
                    if (filter.IS_HIDE_AREA != true)
                    {
                        this.AREA_ID = ss.AREA_ID ?? 0;
                        this.AREA_CODE = ss.AREA_CODE;
                        this.AREA_NAME = ss.AREA_NAME;
                    }
                    if (filter.IS_HIDE_PATY != true)
                    {
                        this.SS_PATIENT_TYPE_ID = ss.SS_PATIENT_TYPE_ID ?? 0;
                        this.SS_PATIENT_TYPE_NAME = ss.SS_PATIENT_TYPE_NAME;
                    }
                }
                else
                {
                    decimal DepBil = (data.TRANSACTION_TIME >= filter.TIME_FROM && data.TRANSACTION_TIME <= filter.TIME_TO) ? (data.AMOUNT - (data.EXEMPTION ?? 0) - (data.KC_AMOUNT ?? 0) - totalFund) : 0;
                    decimal Repay = (data.CANCEL_TIME >= filter.TIME_FROM && data.CANCEL_TIME <= filter.TIME_TO) ? (data.AMOUNT - (data.EXEMPTION ?? 0) - (data.KC_AMOUNT ?? 0) - totalFund) : 0;
                    this.TOTAL_DEPOSIT_BILL_AMOUNT = DepBil;
                    if (filter.IS_ADD_BILL_CANCEL == true && data.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        this.TOTAL_REPAY_AMOUNT = Repay;
                    }
                    this.DEP_BIL_TRANSFER = data.TRANSFER_AMOUNT ?? 0;
                }

                if (data.TRANSACTION_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU)
                {
                    this.DEPOSIT_BILL_NUM_ORDER = data.NUM_ORDER;
                }
                else
                    this.REPAY_NUM_ORDER = data.NUM_ORDER;
            }
        }

        public Mrs00289RDO(V_HIS_TRANSACTION data, V_HIS_TRANSACTION preCash, V_HIS_PATIENT_TYPE_ALTER patyAlter, long type, Mrs00289Filter filter, Mrs00296RDO ss)
        {
            if (data != null)
            {
                this.IS_ACTIVE = data.IS_ACTIVE;
                this.ACCOUNT_BOOK_CODE = data.ACCOUNT_BOOK_CODE;
                this.ACCOUNT_BOOK_ID = data.ACCOUNT_BOOK_ID;
                this.ACCOUNT_BOOK_NAME = data.ACCOUNT_BOOK_NAME;
                this.PAY_FORM_ID = data.PAY_FORM_ID;
                this.PAY_FORM_NAME = data.PAY_FORM_NAME;
                this.PAY_FORM_CODE = data.PAY_FORM_CODE;
                this.BANK_CARD_CODE = data.TDL_BANK_CARD_CODE;
                this.TRANSACTION_CODE = data.TRANSACTION_CODE;
                this.TIG_TRANSACTION_CODE = data.TIG_TRANSACTION_CODE;
                this.CASHIER_LOGINNAME = data.CASHIER_LOGINNAME;
                if (filter.IS_HIDE_CASHIER_ROOM != true)
                {
                    this.CASHIER_ROOM_ID = data.CASHIER_ROOM_ID;
                    this.CASHIER_ROOM_CODE = data.CASHIER_ROOM_CODE;
                    this.CASHIER_ROOM_NAME = data.CASHIER_ROOM_NAME;
                }
                this.CASHIER_USERNAME = data.CASHIER_USERNAME;
                this.PRE_CASHIER_LOGINNAME = data.CASHIER_LOGINNAME;
                this.PRE_CASHIER_USERNAME = data.CASHIER_USERNAME;
                this.CREATE_TIME = data.TRANSACTION_TIME;
                this.TRANSACTION_DATE = data.TRANSACTION_DATE;
                this.CREATE_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(data.TRANSACTION_TIME);
                this.DOB = data.TDL_PATIENT_DOB ?? 0;
                this.PATIENT_CODE = data.TDL_PATIENT_CODE;
                this.DEPOSIT_BILL_TRANSACTION_CODE = data.TRANSACTION_CODE;
                this.TREATMENT_ID = data.TREATMENT_ID ?? 0;
                this.VIR_PATIENT_NAME = data.TDL_PATIENT_NAME;
                this.TREATMENT_CODE = data.TREATMENT_CODE;
                this.TRANSACTION_TYPE_CODE = data.TRANSACTION_TYPE_CODE;
                this.IS_CANCEL = data.IS_CANCEL;
                if (patyAlter != null)
                {
                    this.PATIENT_TYPE_ID = patyAlter.PATIENT_TYPE_ID;
                    this.PATIENT_TYPE_NAME = patyAlter.PATIENT_TYPE_NAME;
                }

                if (preCash != null)
                {
                    this.PRE_CASHIER_LOGINNAME = preCash.CASHIER_LOGINNAME;
                    this.PRE_CASHIER_USERNAME = preCash.CASHIER_USERNAME;
                }

                if (data.TRANSACTION_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU)
                {
                    this.DEPOSIT_BILL_NUM_ORDER = data.NUM_ORDER;
                }
                else
                {
                    this.REPAY_NUM_ORDER = data.NUM_ORDER;
                }

                if (type == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU)
                {
                    this.TRANSACTION_TYPE_NAME_PLUS = "Tạm thu dịch vụ";
                    if (ss != null)
                    {
                        this.TOTAL_DEPOSIT_BILL_AMOUNT = ss.TOTAL_DEPOSIT_BILL_PRICE;
                        this.PATIENT_BHYT_PRICE = ss.TOTAL_PATIENT_BHYT_PRICE;
                        this.CHENH_LECH = ss.TOTAL_DEPOSIT_BILL_PRICE - ss.TOTAL_PATIENT_BHYT_PRICE;
                        if (data.AMOUNT > 0)
                        {
                            this.DEP_BIL_TRANSFER = ss.TOTAL_DEPOSIT_BILL_PRICE * (data.TRANSFER_AMOUNT ?? 0) / data.AMOUNT;
                        }
                        if (filter.IS_HIDE_AREA != true)
                        {
                            this.AREA_ID = ss.AREA_ID ?? 0;
                            this.AREA_CODE = ss.AREA_CODE;
                            this.AREA_NAME = ss.AREA_NAME;
                        }
                        if (filter.IS_HIDE_PATY != true)
                        {
                            this.SS_PATIENT_TYPE_ID = ss.SS_PATIENT_TYPE_ID ?? 0;
                            this.SS_PATIENT_TYPE_NAME = ss.SS_PATIENT_TYPE_NAME;
                        }
                    }
                    else
                    {
                        this.TOTAL_DEPOSIT_BILL_AMOUNT = data.AMOUNT;
                        this.DEP_BIL_TRANSFER = data.TRANSFER_AMOUNT ?? 0;
                    }
                }
                else if (type == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU)
                {
                    this.TRANSACTION_TYPE_NAME_PLUS = "Hoàn thu dịch vụ";
                    if (ss != null)
                    {
                        this.TOTAL_REPAY_AMOUNT = ss.TOTAL_REPAY_PRICE;
                        if (filter.IS_HIDE_AREA != true)
                        {
                            this.AREA_ID = ss.AREA_ID ?? 0;
                            this.AREA_CODE = ss.AREA_CODE;
                            this.AREA_NAME = ss.AREA_NAME;
                        }
                        if (filter.IS_HIDE_PATY != true)
                        {
                            this.SS_PATIENT_TYPE_ID = ss.SS_PATIENT_TYPE_ID ?? 0;
                            this.SS_PATIENT_TYPE_NAME = ss.SS_PATIENT_TYPE_NAME;
                        }
                    }
                    else
                    {
                        this.TOTAL_REPAY_AMOUNT = data.AMOUNT;
                    }
                }
                else
                {
                    this.TRANSACTION_TYPE_NAME_PLUS = data.TRANSACTION_TYPE_NAME;
                    if (ss != null)
                    {
                        this.TOTAL_DEPOSIT_BILL_AMOUNT = ss.TOTAL_DEPOSIT_BILL_PRICE;
                        if (data.AMOUNT > 0)
                        {
                            this.DEP_BIL_TRANSFER = ss.TOTAL_DEPOSIT_BILL_PRICE * (data.TRANSFER_AMOUNT ?? 0) / data.AMOUNT;
                        }
                        if (filter.IS_HIDE_AREA != true)
                        {
                            this.AREA_ID = ss.AREA_ID ?? 0;
                            this.AREA_CODE = ss.AREA_CODE;
                            this.AREA_NAME = ss.AREA_NAME;
                        }
                    }
                    else
                    {
                        this.TOTAL_DEPOSIT_BILL_AMOUNT = data.AMOUNT;
                        this.DEP_BIL_TRANSFER = data.TRANSFER_AMOUNT ?? 0;
                    }
                }
            }
        }

        public decimal DEP_BIL_TRANSFER { get; set; }

        public long LOG_TIME { get; set; }
    }

    public class CARD
    {
        public string BANK_CARD_CODE { get; set; }
    }
}

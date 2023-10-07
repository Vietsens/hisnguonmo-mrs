using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00516
{
    public class Mrs00516RDO
    {
        public Mrs00516RDO(V_HIS_TRANSACTION r, List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter, List<HIS_DEPARTMENT_TRAN> ListDepartmentTran, List<HIS_ACCOUNT_BOOK> ListAccountBook)
        {
            var departmentTran = ListDepartmentTran.LastOrDefault(o => o.TREATMENT_ID == r.TREATMENT_ID && o.DEPARTMENT_IN_TIME <= r.CREATE_TIME) ?? ListDepartmentTran.FirstOrDefault(o => o.TREATMENT_ID == r.TREATMENT_ID && o.DEPARTMENT_IN_TIME >r.CREATE_TIME) ?? new HIS_DEPARTMENT_TRAN();
            var patientTypeAlter = listPatientTypeAlter.FirstOrDefault(o => o.TREATMENT_ID == r.TREATMENT_ID && o.HEIN_CARD_NUMBER != null) ?? new HIS_PATIENT_TYPE_ALTER();
            this.TDL_PATIENT_CODE = r.TDL_PATIENT_CODE;
            this.CASHIER_LOGINNAME = r.CASHIER_LOGINNAME;
            this.TRANSACTION_TIME = r.TRANSACTION_TIME;
            this.TRANSACTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(r.TRANSACTION_TIME);
            this.TRANSACTION_TYPE_ID = r.TRANSACTION_TYPE_ID;
            this.CASHIER_USERNAME = r.CASHIER_USERNAME;
            this.TREATMENT_CODE = r.TREATMENT_CODE;
            this.PATIENT_CODE = r.TDL_PATIENT_CODE;
            this.PATIENT_NAME = r.TDL_PATIENT_NAME;
            this.PATIENT_ADDRESS = r.TDL_PATIENT_ADDRESS;
            this.EINVOICE_NUM_ORDER = r.EINVOICE_NUM_ORDER;
            this.TRANSFER_AMOUNT = r.TRANSFER_AMOUNT??0;
            this.IS_TRANSFER = (r.PAY_FORM_ID != IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__TM) ? (short)1 : (short)0;
            if (r.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
            {
                this.IS_CANCEL = 1;
                this.STATUS = "Đã hủy";
            }
            else
            {
                this.IS_CANCEL = 0;
                this.STATUS = "";
            }
            
            this.INVOICE_CODE = r.INVOICE_CODE;
            this.INVOICE_SYS = r.INVOICE_SYS;
            this.HEIN_CARD_NUMBER = patientTypeAlter.HEIN_CARD_NUMBER;
            this.DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentTran.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;

            if (r.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT)
            {

                if (r.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                {
                    this.TOTAL_BILL_CANCEL = r.AMOUNT;
                }
                else
                {
                    this.BILL = r.AMOUNT;
                    this.KC_AMOUNT = r.KC_AMOUNT ?? 0;
                    this.TOTAL_BILL_AMOUNT = r.AMOUNT;
                    this.TOTAL_EXEMPTION = r.EXEMPTION ?? 0;
                    this.TOTAL_BILL_FUND = r.TDL_BILL_FUND_AMOUNT ?? 0;
                    if (r.PAY_FORM_ID != IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__TM)
                    {
                        this.TOTAL_BILL_AMOUNT_CK = r.AMOUNT;
                        this.KC_AMOUNT_CK = r.KC_AMOUNT ?? 0;
                        this.TOTAL_BILL_FUND_CK = r.TDL_BILL_FUND_AMOUNT ?? 0;
                    }
                }

            }
            else if (r.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU)
            {

                if (r.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                {
                    this.TOTAL_DEPOSIT_CANCEL = r.AMOUNT;
                }
                else
                {
                    this.TOTAL_DEPOSIT_AMOUNT = r.AMOUNT;
                    if (r.PAY_FORM_ID != IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__TM)
                    {
                        TOTAL_DEPOSIT_AMOUNT_CK = r.AMOUNT;
                    }
                    this.DEPOSIT = r.AMOUNT;
                }
            }
            else if (r.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU)
            {

                if (r.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                {
                    this.TOTAL_REPAY_CANCEL = r.AMOUNT;
                }
                else
                {
                    this.TOTAL_REPAY_AMOUNT = r.AMOUNT;
                    if (r.PAY_FORM_ID != IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__TM)
                    {
                        TOTAL_REPAY_AMOUNT_CK = r.AMOUNT;
                    }
                    this.REPAY = r.AMOUNT;
                }
            }

            if (ListAccountBook != null)
            {
                var accountBook = ListAccountBook.FirstOrDefault(o => o.ID == r.ACCOUNT_BOOK_ID);
                if (accountBook != null)
                {
                    this.ACCOUNT_BOOK_CODE = accountBook.ACCOUNT_BOOK_CODE;
                    this.ACCOUNT_BOOK_NAME = accountBook.ACCOUNT_BOOK_NAME;
                    this.TEMPLATE_CODE = accountBook.TEMPLATE_CODE;
                }
            }
            this.DESCRIPTION = r.DESCRIPTION;
            this.NUM_ORDER_STR = string.Format("{0:0000000}", Convert.ToInt64(r.NUM_ORDER));
            this.TRANSACTION_CODE = r.TRANSACTION_CODE;
            this.PAY_FORM_CODE = r.PAY_FORM_CODE;
            this.PAY_FORM_NAME = r.PAY_FORM_NAME;
            this.SYMBOL_CODE = r.SYMBOL_CODE;
            this.SALE_TYPE_ID = r.SALE_TYPE_ID;

        }

        public Mrs00516RDO()
        {
            // TODO: Complete member initialization
        }
        public string PATIENT_CODE { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string CASHIER_LOGINNAME { get; set; }
        public long TRANSACTION_TIME { get; set; }
        public string TRANSACTION_TIME_STR { get; set; }
        public long TRANSACTION_TYPE_ID { get; set; }
        public string CASHIER_USERNAME { get; set; }
        public decimal TOTAL_BILL_AMOUNT { get; set; }
        public decimal TOTAL_BILL_AMOUNT_CK { get; set; }
        public decimal TOTAL_DEPOSITS_AMOUNT { get; set; }
        public decimal TOTAL_REPAYS_AMOUNT { get; set; }
        public decimal TOTAL_DEPOSIT_AMOUNT { get; set; }
        public decimal TOTAL_DEPOSIT_AMOUNT_CK { get; set; }
        public decimal TOTAL_REPAY_AMOUNT { get; set; }
        public decimal TOTAL_REPAY_AMOUNT_CK { get; set; }
        public decimal KC_AMOUNT { get; set; }
        public decimal KC_AMOUNT_CK { get; set; }
        public decimal TOTAL_EXEMPTION { get; set; }
        public decimal TOTAL_BILL_FUND { get; set; }
        public decimal TOTAL_BILL_FUND_CK { get; set; }
        public string ACCOUNT_BOOK_CODE { set; get; }
        public string ACCOUNT_BOOK_NAME { set; get; }
        public string TEMPLATE_CODE { set; get; }
        public string NUM_ORDER_STR { get; set; }
        public string DESCRIPTION { get; set; }//lý do tạm ứng, ghi chú thanh toán
        public string EINVOICE_NUM_ORDER { get; set; }
        public string INVOICE_CODE { get; set; }
        public string INVOICE_SYS { get; set; }
        public string SYMBOL_CODE { set; get; }

        public string TRANSACTION_CODE { get; set; }

        public string PAY_FORM_CODE { get; set; }//hình thức thanh toán

        public string PAY_FORM_NAME { get; set; }

        public decimal TOTAL_BILL_CANCEL { get; set; }
        public decimal TOTAL_DEPOSIT_CANCEL { get; set; }
        public decimal TOTAL_REPAY_CANCEL { get; set; }
        public long? SALE_TYPE_ID { get; set; }

        public string PATIENT_ADDRESS { get; set; }
        public long IS_CANCEL { get; set; }
        public string STATUS { get; set; }

        public decimal DEPOSIT { get; set; }

        public decimal REPAY { get; set; }

        public decimal BILL { get; set; }

        public string TDL_PATIENT_CODE { get; set; }

        public decimal TRANSFER_AMOUNT { get; set; }

        public short IS_TRANSFER { get; set; }
    }
}

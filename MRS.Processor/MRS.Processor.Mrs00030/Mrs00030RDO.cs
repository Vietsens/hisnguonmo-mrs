using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00030
{
    public class Mrs00030RDO
    {
        public string CASHIER_LOGINNAME { get; set; }
        public string CASHIER_USERNAME { get; set; }
        public decimal TOTAL_BILL_AMOUNT { get; set; }
        public decimal TOTAL_DEPOSITS_AMOUNT { get; set; }
        public decimal TOTAL_REPAYS_AMOUNT { get; set; }
        public decimal TOTAL_DEPOSIT_AMOUNT { get; set; }
        public decimal TOTAL_REPAY_AMOUNT { get; set; }
        public decimal KC_AMOUNT { get; set; }
        public decimal TOTAL_EXEMPTION { get; set; }
        public decimal TOTAL_BILL_AMOUNT_BEFORE { get; set; }
        public decimal TOTAL_BILL_AMOUNT_AFTER { get; set; }
    }

    public class Mrs00030RDODetail : HIS_SERE_SERV
    {
        public string CASHIER_LOGINNAME { get; set; }
        public string CASHIER_USERNAME { get; set; }
        public long TRANSACTION_TIME { get; set; }
        public string TRANSACTION_TIME_STR { get; set; }
        public string TDL_PATIENT_ADDRESS { get; set; }
        public string TDL_PATIENT_CODE { get; set; }
        public string TDL_PATIENT_DOB { get; set; }
        public string TDL_PATIENT_GENDER_NAME { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public string TDL_REQUEST_ROOM_NAME { get; set; }
        public string TDL_SERVICE_UNIT_NAME { get; set; }
        public string TDL_SERVICE_TYPE_NAME { get; set; }
        public string TRANSACTION_TYPE_CODE { get; set; }
        public string TRANSACTION_TYPE_NAME { get; set; }
        public string PAY_FORM_CODE { get; set; }
        public string PAY_FORM_NAME { get; set; }

        public Mrs00030RDODetail(HIS_SERE_SERV data, V_HIS_TRANSACTION transaction)
        {
            if (data != null)
            {
                Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00030RDODetail>(this, data);
            }

            if (transaction != null)
            {
                this.CASHIER_LOGINNAME = transaction.CASHIER_LOGINNAME;
                this.CASHIER_USERNAME = transaction.CASHIER_USERNAME;
                this.TRANSACTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(transaction.TRANSACTION_TIME);
                this.TRANSACTION_TIME = transaction.TRANSACTION_TIME;
                this.TDL_PATIENT_ADDRESS = transaction.TDL_PATIENT_ADDRESS;
                this.TDL_PATIENT_CODE = transaction.TDL_PATIENT_CODE;
                if (transaction.TDL_PATIENT_IS_HAS_NOT_DAY_DOB == 1)
                {
                    this.TDL_PATIENT_DOB = transaction.TDL_PATIENT_DOB.HasValue ? transaction.TDL_PATIENT_DOB.ToString().Substring(0, 4) : "";
                }
                else
                {
                    this.TDL_PATIENT_DOB = Inventec.Common.DateTime.Convert.TimeNumberToDateString(transaction.TDL_PATIENT_DOB ?? 0);
                }

                this.TDL_PATIENT_GENDER_NAME = transaction.TDL_PATIENT_GENDER_NAME;
                this.TDL_PATIENT_NAME = transaction.TDL_PATIENT_NAME;
                this.TRANSACTION_TYPE_CODE = transaction.IS_DEBT_COLLECTION ==IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE?"00":transaction.TRANSACTION_TYPE_CODE;
                this.TRANSACTION_TYPE_NAME = transaction.IS_DEBT_COLLECTION == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE ? "Thu nợ" : transaction.TRANSACTION_TYPE_NAME;
                this.PAY_FORM_CODE = transaction.PAY_FORM_CODE;
                this.PAY_FORM_NAME = transaction.PAY_FORM_NAME;
            }
        }

        public Mrs00030RDODetail()
        {
        }
    }
}

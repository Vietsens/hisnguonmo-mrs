using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00812
{
    public class Mrs00812RDO
    {
        public long INVOICE_NUMBER { get; set; }
        public long NUM_ORDER { set; get; }
        public string DATE { set; get; }
        public long TREATMENT_ID { set; get; }
        public string TREATMENT_CODE { set; get; }
        public long ROOM_ID { set; get; }
        public string ROOM_CODE { get; set; }
        public string ROOM_NAME { set; get; }
        public long DEPARTMENT_ID { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string  DEPARTMENT_NAME { get; set; }
        public long PATIENT_ID { set; get; }
        public string TDL_PATIENT_NAME { set; get; }
        public string TDL_PATIENT_CODE { set; get; }
        public decimal PRICE { set; get; }
        public decimal AMOUNT { set; get; }
        public long SERVICE_ID { set; get; }
        public string TDL_SERVICE_NAME { set; get; }
        public long SERVICE_REQ_TYPE_ID { set; get; }
        public string SERVICE_CODE { set; get; }
        public decimal TOTAL_PRICE_AN_CA { set; get; }
        public decimal TOTAL_PRICE_AN_DV { set; get; }
        public decimal TOTAL_PRICE_AN_DAYCARE { set; get; }
        public decimal TOTAL_PRICE_QTT { get; set; }
        public decimal TOTAL_PRICE_QT { set; get; }
        public long? SERVICE_REQ_ID {set;get;}
        public string INVOICE_CODE { set; get; }
        public string TDL_HEIN_CARD_NUMBER { get; set; }
        public long? INTRUCTION_TIME { set; get; }
        public string CASHIER_USERNAME { get; set; }
        public string CASHIER_LOGINNAME { get; set; }
        public string TRANSACTION_TYPE_NAME { get; set; }
        public long? PARENT_ID { get; set; }
        public string SERVICE_PARENT_CODE { get; set; }
        public string SERVICE_PARENT_NAME { get; set; }

        public long ACCOUNT_BOOK_ID { set; get; }
        public string ACCOUNT_BOOK_CODE { set; get; }
        public string ACCOUNT_BOOK_NAME { get; set; }
        public long PAY_FORM_ID { set; get; }
        public string PAY_FORM_CODE { set; get; }
        public string PAY_FORM_NAME { get; set; }

        public decimal TOTAL_PRICE { set; get; }
        public Dictionary<string, decimal> DIC_PAR_TOTAL_PRICE { get; set; }
    }
}

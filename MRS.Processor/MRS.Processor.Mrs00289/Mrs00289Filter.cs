using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00289
{
    public class Mrs00289Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }
        public long? PAY_FORM_ID { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public long? SS_PATIENT_TYPE_ID { get; set; }

        public long? BRANCH_ID { get; set; }
        public string LOGINNAME { get; set; }
        public string CASHIER_LOGINNAME { get; set; }

        public List<long> EXACT_CASHIER_ROOM_IDs { get; set; }

        public List<long> PAY_FORM_IDs { get; set; }

        public List<long> AREA_IDs { get; set; }
        public bool? IS_MOV_CLS_TO_PARENT { get; set; }
        public bool? IS_ADD_INFO_AREA { get; set; }
        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }
        public bool? IS_HIDE_PATY { get; set; }
        public bool? IS_HIDE_AREA { get; set; }
        public bool? IS_HIDE_CASHIER_ROOM { get; set; }
        public bool? IS_HIDE_TRANSACTION { get; set; }
        public List<long> TRANSACTION_TYPE_IDs { get; set; }
        public bool? IS_REPAY_SERVICE { get; set; }
        public bool? ADD__SALE_EXP { get; set; }
        public bool? IS_CHANGE_TO_BHYT { set; get; }
        public bool? IS_HOLIDAYS { get; set; }
       public bool? IS_NOT_HOLIDAYS { get; set; }

       public bool IS_DESCENDING { get; set; }

       public bool IS_ASCENDING { get; set; }
       public string CHANGE_TYPE_TO_TYPE { set; get; }

       public bool? IS_ADD_BILL_CANCEL { get; set; }

       public bool? IS_ADD_DEPO_CANCEL { get; set; }

       public List<short> INPUT_DATA_ID_PRICE_TYPEs { get; set; } //nguồn tiền thanh toán: 1: Đồng chi trả; 2: Tự trả và viện phí
       public bool? IS_PATIENT_TYPE { get; set; }

       public long? INPUT_DATA_ID_BC_TYPE { get; set; }
    }
}

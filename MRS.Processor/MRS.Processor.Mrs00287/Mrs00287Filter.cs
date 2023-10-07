using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00287
{
    public class Mrs00287Filter
    {
        public long CREATE_TIME_FROM { get;  set;  }
        public long CREATE_TIME_TO { get;  set;  }
        public long? ACCOUNT_BOOK_ID { get;  set;  }
        public long? BRANCH_ID { get; set; }
        public long? PAY_FORM_ID { get; set; }
        public List<long> PAY_FORM_IDs { get; set; }
        public string CASHIER_LOGINNAME { get; set; }

        public List<long> EXACT_CASHIER_ROOM_IDs { get; set; }

        public List<long> PATIENT_TYPE_IDs { get; set; }
        public List<long> SS_PATIENT_TYPE_IDs { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public List<long> AREA_IDs { get; set; }
        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }
        public bool? IS_MOV_CLS_TO_PARENT { get; set; }
        public bool? IS_ADD_INFO_AREA { get; set; }

        public bool? ADD__SALE_EXP { get; set; }

        public bool? IS_HOLIDAYS { get; set; }

        public bool? IS_NOT_HOLIDAYS { get; set; }

        public bool? IS_ADD_BILL_CANCEL { get; set; }

        public bool? IS_ADD_DEPO_CANCEL { get; set; }

        public bool? IS_NEW_PROFESSION { get; set; }

        public List<short> INPUT_DATA_ID_PRICE_TYPEs { get; set; } //nguồn tiền thanh toán: 1: Đồng chi trả; 2: Tự trả và viện phí

        public long? INPUT_DATA_ID_BC_TYPE { get; set; }

        public short? INPUT_DATA_ID_STTRAN_TYPE { get; set; } //Trang thai: 1: khoa; 2: mo khoa; 3: Tat ca
    }
}

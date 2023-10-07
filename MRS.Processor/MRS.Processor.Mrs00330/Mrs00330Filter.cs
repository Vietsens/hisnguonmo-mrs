using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00330
{
    public class Mrs00330Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public long? SERE_SERV_PATIENT_TYPE_ID { get; set; }
        public string CASHIER_LOGINNAME { get; set; }
        public string LOGINNAME { get; set; }
        public List<long> TRAN_TREATMENT_TYPE_IDs { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public bool? IS_TREATMENT_OUT_NOT_DEPOSIT { get; set; }

        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }

        public List<long> SERVICE_TYPE_IDs { get; set; }
        public List<long> EXACT_PARENT_SERVICE_IDs { get; set; }

        public long? EXACT_CASHIER_ROOM_ID { get; set; }

        /// <summary>
        /// True: chi lay hoa don thuong
        /// False: chi lay hoa don dich vu
        /// Null: lay ca hai
        /// </summary>
        public bool? IS_BILL_NORMAL { get; set; }
        //loc theo hinh thuc thanh toan
        public long? PAY_FORM_ID { get; set; }
        //loc theo hinh thuc thanh toan
        public List<long> PAY_FORM_IDs { get; set; }
        public bool? IS_MERGE_TREATMENT { get; set; }
        public bool? IS_ADD_BILL_CANCEL { get; set; }
        /// <summary>
        /// BILL: Số tiền thanh toán
        /// BNTT: Bệnh nhân tự trả
        /// </summary>
        public string KEY_TYPE_PRICE { get; set; }//Loại tiền
        /// <summary>
        /// PARENT: Dịch vụ cha
        /// CATEGORY:Nhóm báo cáo
        /// </summary>
        public string KEY_TYPE_PAR { get; set; }// loại nhóm

        public bool? ADD_OTHER_SALE_TYPE { get; set; }

        public string KEY_GROUP_DETAIL { get; set; }
    }
}

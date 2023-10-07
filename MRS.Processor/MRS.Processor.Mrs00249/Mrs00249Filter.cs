using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00249
{
    public class Mrs00249Filter
    {
        public long CREATE_TIME_FROM { get;  set;  }
        public long CREATE_TIME_TO { get; set; }
        public long? ACCOUNT_BOOK_ID { get; set; }
        public string CASHIER_LOGINNAME { get; set; }
        public List<string> CASHIER_LOGINNAMEs { get; set; }
        public List<long> EXACT_CASHIER_ROOM_IDs { get; set; }
        public long? EXACT_CASHIER_ROOM_ID { get; set; }
        /// <summary>
        /// True: chi lay hoa don thuong
        /// False: chi lay hoa don dich vu
        /// Null: lay ca hai
        /// </summary>
        public bool? IS_BILL_NORMAL { get; set; }
        public short? INPUT_DATA_ID_SALE_TYPE { get; set; }

        public long? BRANCH_ID { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }

        public bool? TAKE_CANCEL { get; set; }

        public bool? IS_ONLY_CANCEL { get; set; }

        public bool? IS_REMOVE_DUPPLICATE { get; set; }

        public bool? TAKE_ALL_DISCOUNT { get; set; }

        public bool? TAKE_SSB { get; set; }

        //public bool? IS_NOT_SPLIT_ROOM { get; set; }

        //public bool? IS_NOT_SPLIT_DEPA { get; set; }

        public bool? IS_EXAM_REQ_TO_EXE_ROOM { get; set; }

        public bool? ADD_CANCEL_INFO { get; set; }

        public bool? ADD_SSB_INFO { get; set; }

        public List<long> PATIENT_TYPE_IDs { get; set; }
        public List<long> TDL_PATIENT_TYPE_IDs { get; set; }

        public List<long> ACCOUNT_BOOK_IDs { get; set; }

        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }

        public List<long> PAY_FORM_IDs { get; set; }

        public List<long> DEPARTMENT_IDs { get; set; }

        public bool? IS_GROUP_BILL { get; set; }

        public List<long> PATIENT_CLASSIFY_IDs { get; set; }

        public List<long> TDL_TREATMENT_TYPE_IDs { get; set; } //diện điều trị của bệnh nhân

        //public bool? IS_ADD_CLINICAL_DEPA { get; set; }

        public long? LIMIT_TIME { get; set; }//giới hạn thời gian hoàn ứng được tính cho thanh toán (phút)

        public List<long> TRANSACTION_TYPE_IDs { get; set; } //diện điều trị của bệnh nhân

        public bool? LOAI_CAN_TRU { get; set; }

        public short? INPUT_DATA_ID_STTRAN_TYPE { get; set; } //Trang thai: 1: khoa; 2: mo khoa; 3: Tat ca
    }
}

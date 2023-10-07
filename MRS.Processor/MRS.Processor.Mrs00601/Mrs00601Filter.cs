using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00601
{
    public class Mrs00601Filter
    {
        public long? FEE_LOCK_TIME_FROM { get; set; }
        public long? FEE_LOCK_TIME_TO { get; set; }
        public long? TRANSACTION_TIME_FROM { get; set; }
        public long? TRANSACTION_TIME_TO { get; set; }
        public string CASHIER_LOGINNAME { get; set; }

        public long? EXACT_CASHIER_ROOM_ID { get; set; }

        public long? ACCOUNT_BOOK_ID { get; set; }
        public long? BRANCH_ID { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public long? SESE_PATIENT_TYPE_ID { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }

        public List<long> END_DEPARTMENT_IDs { get; set; }

        public List<long> EXAM_ROOM_IDs { get; set; }

        public List<long> REQUEST_ROOM_IDs { get; set; }

        public bool IS_DETAIL_DEPA_RO { get; set; }


        /// <summary>
        /// True: chi lay noi tru
        /// False: chi lay ngoai tru
        /// Null: lay ca hai
        /// </summary>
        public bool? IS_TREAT { get; set; }

        public string CATEGORY_CODE__SBA { get; set; }
    }
}

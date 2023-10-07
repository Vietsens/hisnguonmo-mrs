using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00676
{
    public class Mrs00676Filter
    {
        public long CREATE_TIME_FROM { get;  set;  }
        public long CREATE_TIME_TO { get; set; }
        public long? ACCOUNT_BOOK_ID { get; set; }
        public string CASHIER_LOGINNAME { get; set; }
        public long? BRANCH_ID { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public bool? TAKE_CANCEL { get; set; }


        public long? EXACT_CASHIER_ROOM_ID { get; set; }

        /// <summary>
        /// True: chi lay hoa don thuong
        /// False: chi lay hoa don dich vu
        /// Null: lay ca hai
        /// </summary>
        public bool? IS_BILL_NORMAL { get; set; }
    }
}

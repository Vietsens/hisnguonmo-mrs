using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00690
{
    public class Mrs00690Filter
    {
        public long TIME_FROM { get;  set;  }                 // thời gian thực hiện giao địch
        public long TIME_TO { get;  set;  }

        public long? BRANCH_ID { get; set; }                // chi nhánh   
        public string CASHIER_LOGINNAME { get; set; }       // thu ngân   
        public long? EXACT_CASHIER_ROOM_ID { get; set; }       // phòng thu ngân   
        public long? ACCOUNT_BOOK_ID { get; set; }       // Số thu chi
        

        public Mrs00690Filter() { }

        public bool? IS_BILL_NORMAL { get; set; }
        public List<long> SERVICE_IDs { get; set; }//dich vuj ki thuat

        public List<long> SERVICE_TYPE_IDs { get; set; }
        public List<long> EXACT_PARENT_SERVICE_IDs { get; set; }
        public List<long> ACCOUNT_BOOK_IDs { get; set; }//Quyển sổ

        public List<long> WORKING_SHIFT_IDs { get; set; }
    }
}

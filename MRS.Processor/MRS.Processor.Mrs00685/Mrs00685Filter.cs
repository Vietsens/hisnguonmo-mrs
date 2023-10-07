using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00685
{
    public class Mrs00685Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }                // chi nhánh   
        public string CASHIER_LOGINNAME { get; set; }       // thu ngân   
        public long? EXACT_CASHIER_ROOM_ID { get; set; }       // phòng thu ngân   
        public long? ACCOUNT_BOOK_ID { get; set; }       // Số thu chi
    }
}

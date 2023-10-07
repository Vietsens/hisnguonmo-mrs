using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00684
{
    public class Mrs00684Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public string CASHIER_LOGINNAME { get; set; }

        public long? EXACT_CASHIER_ROOM_ID { get; set; }

        public long? ACCOUNT_BOOK_ID { get; set; }

        public List<long> ACCOUNT_BOOK_IDs { get; set; }

        public bool? IS_TREAT_IN { get; set; }
    }
}

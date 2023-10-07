using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00644
{
    public class Mrs00644Filter
    {
        public long? INVOICE_BOOK_ID { get; set; } 
        public string NUM_ORDER_FROM { get; set; }
        public string NUM_ORDER_TO { get; set; }
        public long? CREATE_TIME_FROM { get; set; }
        public long? CREATE_TIME_TO { get; set; }
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public string CASHIER_LOGINNAME { get; set; }
        public string CATEGORY_CODEs { get; set; }
        public bool? HAS_SERE_SERV { get; set; }
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00674
{
    public class Mrs00674Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? MEDI_STOCK_ID { get; set; }

        public List<long> EXAM_ROOM_IDs { get; set; }
        public long? EXACT_EXT_CASHIER_ROOM_ID { get; set; }
    }
}

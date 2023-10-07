using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00399
{
    public class Mrs00399Filter
    {
        public long MONTH { get; set; }
        public long? BED_ID { get; set; }
        public long? EXACT_BED_ROOM_ID { get; set; }
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00604
{
    public class Mrs00604Filter
    {
        public long IN_TIME_FROM { get; set; }
        public long IN_TIME_TO { get; set; }
        public List<string> CATEGORY_CODE__KSKs { get; set; }
        public List<string> ROOM_CODE__PCBs { get; set; }
    }
}

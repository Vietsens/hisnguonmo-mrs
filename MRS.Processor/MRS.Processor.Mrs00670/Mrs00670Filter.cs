using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00670
{
    public class Mrs00670Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public List<long> EXAM_ROOM_IDs { get; set; }//chobn phong thuc hien
    }
}

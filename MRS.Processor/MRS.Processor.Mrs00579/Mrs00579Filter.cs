using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00579
{
    public class Mrs00579Filter
    {
        public long CREATE_TIME_FROM { get; set; }
        public long CREATE_TIME_TO { get; set; }

        public long? EXE_ROOM_ID { get; set; }

        public List<long> REQUEST_ROOM_IDs { get; set; }

        public long? EXACT_EXECUTE_ROOM_ID__EXE { get; set; }

        public List<long> EXACT_EXECUTE_ROOM_IDs__REQ { get; set; }
    }
}
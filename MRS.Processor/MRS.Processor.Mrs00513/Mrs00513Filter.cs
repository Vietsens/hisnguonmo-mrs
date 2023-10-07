using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00513
{
    public class Mrs00513Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }

        public long? REQ_DEPARMTENT_ID { get; set; }
        public long? EXE_DEPARMTENT_ID { get; set; }
    }
}

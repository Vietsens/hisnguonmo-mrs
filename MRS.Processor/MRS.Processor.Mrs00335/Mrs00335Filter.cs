using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00335
{
    public class Mrs00335Filter
    {
        public long? TIME_FROM { get;  set;  }
        public long? TIME_TO { get;  set;  }
        public List<long> ROOM_IDs { get;  set;  }

        public long? FINISH_TIME_FROM { get; set; }

        public long? FINISH_TIME_TO { get; set; }

        public long? EXECUTE_ROOM_ID { get; set; }
    }
}

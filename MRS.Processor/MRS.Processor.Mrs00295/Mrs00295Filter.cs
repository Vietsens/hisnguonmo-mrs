using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00295
{
    public class Mrs00295Filter
    {
        public long? TIME_FROM { get;  set;  }
        public long? TIME_TO { get;  set;  }

        public long? BRANCH_ID { get;  set;  }
        public long? SERVICE_TYPE_ID { get;  set;  }
        public long? SERVICE_ID { get; set; }
        public long? REQUEST_DEPARTMENT_ID { get; set; }
        public long? REQUEST_ROOM_ID { set; get; }
        public Mrs00295Filter() { }
    }
}

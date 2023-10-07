using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00458
{
    public class Mrs00458Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public long? EXAM_ROOM_ID { get;  set;  }
        public long? BRANCH_ID { get;  set;  }
        //public long ROOM_ID { get;  set;  }
    }
}

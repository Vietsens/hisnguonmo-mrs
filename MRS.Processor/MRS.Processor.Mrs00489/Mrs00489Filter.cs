using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00489
{
    public class Mrs00489Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public long? REQUEST_DEPARTMENT_ID { get;  set;  }
        public long? EXECUTE_DEPARTMENT_ID { get;  set;  }
        public long? EXE_ROOM_ID { get;  set;  }
        public bool CHECK_PT { get;  set;  }
        public bool CHECK_TT { get;  set;  } 
       
    }
}

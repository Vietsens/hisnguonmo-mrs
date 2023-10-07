using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00483
{
    public class Mrs00483Filter
    {
        public long TIME_FROM { get;  set;  }                 // thời gian chỉ định
        public long TIME_TO { get;  set;  }

        public long? EXE_ROOM_ID { get;  set;  }               // phòng thực hiện
        public long? REQ_ROOM_ID { get;  set;  }               // phòng yêu cầu

        public long? EXECUTE_DEPARTMENT_ID { get;  set;  }    // khoa thực hiện
        public long? REQUEST_DEPARTMENT_ID { get;  set;  }    // khoa yêu cầu

        public Mrs00483Filter() { }
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00023
{
    /// <summary>
    /// Filter Báo cáo chi tiết yêu cầu khám
    /// </summary>
    class Mrs00023Filter
    {
        public long? CREATE_TIME_FROM { get;  set;  }
        public long? CREATE_TIME_TO { get;  set;  }
        public long? INTRUCTION_TIME_FROM { get;  set;  }
        public long? INTRUCTION_TIME_TO { get;  set;  }
        public List<long> ROOM_IDs { get;  set;  }

        public Mrs00023Filter()
            : base()
        {
        }
    }
}

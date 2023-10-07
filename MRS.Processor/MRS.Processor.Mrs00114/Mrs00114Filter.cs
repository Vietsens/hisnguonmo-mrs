using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00114
{
    /// <summary>
    /// Báo cáo định mức tiêu hao thuốc vật tư
    /// </summary>
    class Mrs00114Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public long SERVICE_TYPE_ID { get;  set;  }

        public long? DEPARTMENT_ID { get;  set;  }
        public List<long> DEPARTMENT_IDs { get;  set;  }

        public long? ROOM_ID { get;  set;  }
        public List<long> ROOM_IDs { get;  set;  }

        public Mrs00114Filter()
            : base()
        {
        }
    }
}

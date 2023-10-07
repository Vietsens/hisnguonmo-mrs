using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00349
{
    /// <summary>
    /// báo cáo sử dụng thuốc gây nghiện hướng tâm thần
    /// </summary>
    public class Mrs00349Filter : FilterBase
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public long MORNING_HOUR_FROM { get;  set;  }
        public long MORNING_HOUR_TO { get;  set;  }

        public long AFTERNOON_HOUR_FROM { get;  set;  }
        public long AFTERNOON_HOUR_TO { get;  set;  }

        public Mrs00349Filter()
            : base()
        {

        }
    }
}

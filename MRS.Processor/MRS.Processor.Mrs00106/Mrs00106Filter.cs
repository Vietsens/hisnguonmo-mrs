using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00106
{
    /// <summary>
    /// Báo cáo thống kê khám chữa bệnh ngoại trú bhyt 14a
    /// </summary>
    class Mrs00106Filter
    {
        public long BRANCH_ID { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00106Filter()
            : base()
        {
        }
    }
}

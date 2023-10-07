using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00140
{
    /// <summary>
    /// Báo cáo chi tiết thuốc thanh toán bhyt quyết định 9324
    /// </summary>
    class Mrs00140Filter
    {
        public long BRANCH_ID { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
    }
}

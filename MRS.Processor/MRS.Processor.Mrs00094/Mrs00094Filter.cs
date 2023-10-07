using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00094
{
    /// <summary>
    /// Báo cáo chi tiết các giao dịch thanh toán bị hủy
    /// </summary>
    class Mrs00094Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00094Filter()
            : base()
        {
        }
    }
}

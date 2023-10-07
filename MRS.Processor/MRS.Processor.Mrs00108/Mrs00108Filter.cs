using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00108
{
    /// <summary>
    /// Báo cáo tổng hợp doanh thu theo ngày duyệt khóa hfs
    /// </summary>
    class Mrs00108Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00108Filter()
            : base()
        {
        }
    }
}

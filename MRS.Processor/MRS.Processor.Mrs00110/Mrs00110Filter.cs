using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00110
{
    /// <summary>
    /// Báo cáo chi tiết số lượng thuốc xuất cho các khoa theo ngày
    /// </summary>
    class Mrs00110Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00110Filter()
            : base()
        {
        }
    }
}

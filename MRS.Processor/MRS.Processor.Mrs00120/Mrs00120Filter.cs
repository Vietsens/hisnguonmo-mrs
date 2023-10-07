using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00120
{
    /// <summary>
    /// Báo cáo thống kê đầu mối đơn vị
    /// </summary>
    class Mrs00120Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00120Filter()
            : base()
        {
        }
    }
}

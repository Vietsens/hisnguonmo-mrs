using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00087
{
    /// <summary>
    /// Báo cáo doanh thu bảng kê bán lẻ theo phiếu thu thanh toán
    /// </summary>
    class Mrs00087Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00087Filter()
            : base()
        {
        }
    }
}

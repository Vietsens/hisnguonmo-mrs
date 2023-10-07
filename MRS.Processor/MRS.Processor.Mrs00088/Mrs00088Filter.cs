using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00088
{
    /// <summary>
    /// Báo cáo doanh thu bảng ke bán le theo hóa đơn giá trị gia tăng
    /// </summary>
    class Mrs00088Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00088Filter()
            : base()
        {
        }
    }
}

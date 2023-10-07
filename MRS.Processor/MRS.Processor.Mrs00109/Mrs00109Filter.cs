using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00109
{
    /// <summary>
    /// Báo cáo tổng hợp doanh thu chi phí gốc của các loại dịch vụ theo đối tượng thanh toán
    /// </summary>
    class Mrs00109Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00109Filter()
            : base()
        {
        }
    }
}

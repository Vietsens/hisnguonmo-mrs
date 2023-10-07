using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00082
{
    /// <summary>
    /// Báo cáo tổng hợp nhập xuất tồn vật tư theo kỳ dữ liệu
    /// </summary>
    class Mrs00082Filter
    {
        /// <summary>
        /// Ky da duoc chot bat buoc, neu khong truyen vao => = 0 => Khong lay duoc du lieu
        /// </summary>
        public long MEDI_STOCK_PERIOD_ID { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00082Filter()
            : base()
        {
        }
    }
}

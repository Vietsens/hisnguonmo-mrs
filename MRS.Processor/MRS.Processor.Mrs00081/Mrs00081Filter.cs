using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00081
{
    /// <summary>
    /// Báo cáo nhập xuất tồn thuốc số lượng.
    /// </summary>
    class Mrs00081Filter
    {
        /// <summary>
        /// Ky da duoc chot Bat buoc phai co, neu khong co => = 0 => khong tim thay du lieu.
        /// </summary>
        public long MEDI_STOCK_PERIOD_ID { get;  set;  }
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public long? MEDI_STOCK_ID { get;  set;  }

        public Mrs00081Filter()
            : base()
        {
        }
    }
}

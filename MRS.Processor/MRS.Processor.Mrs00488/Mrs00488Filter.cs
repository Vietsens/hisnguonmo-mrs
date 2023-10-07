using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00488
{
    /// <summary>
    /// Filter Báo cáo sử dụng thuốc
    /// </summary>
    class Mrs00488Filter
    {
        /// <summary>
        /// Thoi gian tong hop du lieu
        /// </summary>
        public long? TIME_FROM { get;  set;  }
        public long? TIME_TO { get;  set;  }

        public Mrs00488Filter()
            : base()
        {
        }
    }
}

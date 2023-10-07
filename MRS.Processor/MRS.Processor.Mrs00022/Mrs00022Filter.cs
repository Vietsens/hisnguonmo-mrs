using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00022
{
    /// <summary>
    /// Filter Báo cáo tổng hợp yêu cầu theo phòng khám
    /// </summary>
    class Mrs00022Filter
    {
        /// <summary>
        /// Phong bat buoc
        /// </summary>
        public long ROOM_ID { get;  set;  }
        /// <summary>
        /// Thoi gian tong hop du lieu
        /// </summary>
        public long? TIME_FROM { get;  set;  }
        public long? TIME_TO { get;  set;  }

        public Mrs00022Filter()
            : base()
        {
        }
    }
}

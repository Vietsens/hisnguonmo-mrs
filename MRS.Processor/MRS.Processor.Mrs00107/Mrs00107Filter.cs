using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00107
{
    /// <summary>
    /// Báo cáo tổng hợp số lượng dịch vụ khối cận lâm sàn thực hiện tai phòng theo khoa chỉ định
    /// </summary>
    class Mrs00107Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public long EXECUTE_ROOM_ID { get;  set;  }

        public Mrs00107Filter()
            : base()
        { }
    }
}

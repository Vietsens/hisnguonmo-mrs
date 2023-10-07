using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00182
{
    /// <summary>
    /// Báo cáo thống kê viện phí bệnh nhân ngoại trú (Cho viện Tim TW)
    /// </summary>
    public class Mrs00182Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public long CASHIER_ROOM_ID { get;  set;  }
        public Mrs00182Filter() { }
    }
}

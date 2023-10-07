using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00092
{
    /// <summary>
    /// Báo cáo chi tiết tiền thu tạm ứng của bệnh nhân
    /// </summary>
    class Mrs00092Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00092Filter()
            : base()
        {
        }
    }
}

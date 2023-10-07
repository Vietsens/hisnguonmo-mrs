using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00095
{
    /// <summary>
    /// Báo cáo tổng hợp thu chi theo thu ngân và loại dịch vụ
    /// </summary>
    class Mrs00095Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00095Filter()
            : base()
        {
        }
    }
}

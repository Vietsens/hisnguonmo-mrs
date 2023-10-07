using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00202
{
    /// <summary>
    /// Báo cáo de nghi thanh toan bao hiem y te benh nhan kham chua benh ngoai tru c79a tách đúng trái tuyến, cấp cứu
    /// </summary>
    public class Mrs00202Filter : FilterBase
    {
        public long BRANCH_ID { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00202Filter()
        {
        }
    }
}

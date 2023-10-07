using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00311
{
    /// <summary>
    /// De nghi thanh toan bao hiem y te benh nhan kham chua benh noi tru: File Mem 46 cot
    /// </summary>
    public class Mrs00311Filter : FilterBase
    {
        public long BRANCH_ID { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00311Filter()
            : base()
        {

        }
    }
}

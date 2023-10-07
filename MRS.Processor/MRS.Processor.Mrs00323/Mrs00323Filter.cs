using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00323
{
    /// <summary>
    /// De nghi thanh toan bao hiem y te benh nhan kham chua benh ngoai tru: File Mem 46 cot
    /// </summary>
    public class Mrs00323Filter : FilterBase
    {
        public long MEDI_STOCK_ID { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00323Filter()
            : base()
        {

        }
    }
}

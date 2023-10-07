using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00267
{
    /// <summary>
    /// Báo cáo sổ khám bệnh RAE
    /// </summary>
    public class Mrs00267Filter : FilterBase
    {
        public long MEDI_STOCK_ID { get;  set;  }
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

    }
}

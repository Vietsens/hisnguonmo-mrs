using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00129
{
    /// <summary>
    /// Báo cáo chi tiết Bệnh nhân
    /// </summary>
    class Mrs00129Filter
    {
        public long MEDI_STOCK_ID { get;  set;  }

        public long DATE_FROM { get;  set;  }
        public long DATE_TO { get;  set;  }
    }
}

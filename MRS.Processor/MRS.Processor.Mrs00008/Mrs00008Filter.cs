using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00008
{
    /// <summary>
    /// Filter báo cáo danh sách phát thuốc
    /// </summary>
    class Mrs00008Filter
    {
        public long? CREATE_TIME_FROM { get;  set;  }
        public long? CREATE_TIME_TO { get;  set;  }

        public long? MEDI_STOCK_ID { get;  set;  }

        public Mrs00008Filter()
            : base()
        {
        }
    }
}

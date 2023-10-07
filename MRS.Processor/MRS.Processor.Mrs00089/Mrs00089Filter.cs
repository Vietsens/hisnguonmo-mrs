using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00089
{
    /// <summary>
    /// Báo cáo tổng hợp xuất đơn thuốc -  theo bệnh nhân
    /// </summary>
    class Mrs00089Filter
    {
        public long MEDI_STOCK_ID { get;  set;  }
        public List<long> MEDI_STOCK_IDs { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00089Filter()
            : base()
        {
        }
    }
}

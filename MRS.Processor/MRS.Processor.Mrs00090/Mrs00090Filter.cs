using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00090
{
    /// <summary>
    /// Báo cáo tổng hợp xuất đơn thuốc theo loại thuốc
    /// </summary>
    class Mrs00090Filter
    {
        public long MEDI_STOCK_ID { get;  set;  }
        public List<long> MEDI_STOCK_IDs { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public long? REQ_DEPARTMENT_ID { get; set; }
        public List<long> REQ_DEPARTMENT_IDs { set; get; }
        public Mrs00090Filter()
            : base()
        {
        }
    }
}

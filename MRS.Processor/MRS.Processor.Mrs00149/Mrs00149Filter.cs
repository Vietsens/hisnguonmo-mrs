using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00149
{
    /// <summary>
    /// Báo cáo sử dụng thuốc nhập khẩu
    /// </summary>
    class Mrs00149Filter
    {
        public long MEDI_STOCK_ID { get;  set;  }
        public long DATE_FROM { get;  set;  }
        public long DATE_TO { get;  set;  }
        public long EXP_MEST_STT_ID { get;  set;  }
        public long EXP_MEST_TYPE_ID { get;  set;  }
    }
}

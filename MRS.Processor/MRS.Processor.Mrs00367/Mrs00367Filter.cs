using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00367
{
    /// <summary>
    /// báo cáo số bệnh nhân kết thúc khám
    /// </summary>
    public class Mrs00367Filter : FilterBase
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public long MEDI_STOCK_ID { get;  set;  }
        public long EXP_MEST_REASON_ID { get;  set;  }
        public long EXP_MEST_TYPE_ID { get;  set;  }

        public Mrs00367Filter()
            : base()
        {

        }
    }
}

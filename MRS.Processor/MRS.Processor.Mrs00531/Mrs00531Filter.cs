using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00531
{
    /// <summary>
    /// Báo cáo chi tiết nhập xuất tồn theo các kho
    /// </summary>
    public class Mrs00531Filter : FilterBase
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public bool? IS_MERGE { get; set; }
        public List<string> CLN_MEDI_STOCK_CODEs { get; set; }
        public List<string> UUCLN_MEDI_STOCK_CODEs { get; set; }
        public List<long> MEDI_STOCK_IDs { get;  set;  }
        
        public Mrs00531Filter()
            : base()
        {

        }
    }
}

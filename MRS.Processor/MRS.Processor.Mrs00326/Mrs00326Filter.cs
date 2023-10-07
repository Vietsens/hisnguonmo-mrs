using MOS.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00326
{
    /// <summary>
    /// Báo cáo chi tiết nhập xuất tồn theo các kho
    /// </summary>
    public class Mrs00326Filter: FilterBase
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public List<long> MEDI_STOCK_IDs { get;  set;  }

        public Mrs00326Filter()
            : base()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00185
{
    /// <summary>
    /// Báo cáo chi tiết nhập xuất tồn theo kho (có số thầu, phục vụ cho viện tim trung ương)
    /// </summary>
    public class Mrs00185Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public List<long> BID_IDs { get; set; }

        public List<long> CURRENTBRANCH_MEDI_STOCK_IDs { get; set; }

        public Mrs00185Filter()
        { }
    }
}

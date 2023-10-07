using MOS.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00199
{
    /// <summary>
    /// Báo cáo thống kê thuốc thanh toán bhyt 20 - tách tuyến thêm nhóm khác
    /// </summary>
    public class Mrs00199Filter : FilterBase
    {
        public long BRANCH_ID { get; set; }

        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }

        public Mrs00199Filter() { }
    }
}

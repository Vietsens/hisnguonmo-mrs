using MOS.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00196
{
    /// <summary>
    /// Báo cáo thông kê chi phí khám chữa bệnh nội trú bhyt 14b
    /// </summary>
    public class Mrs00196Filter : FilterBase
    {
        public long BRANCH_ID { get; set; }

        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }

        public List<long> BRANCH_IDS { get; set; }

        public Mrs00196Filter()
        {

        }
    }
}

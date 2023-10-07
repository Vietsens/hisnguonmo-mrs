using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00203
{
    /// <summary>
    /// Báo cáo đề nghị thanh toán bhyt bệnh nhân kcb nội trú C80a - tách đúng tuyến, trái tuyến, cấp cứu
    /// </summary>
    public class Mrs00203Filter
    {
        public long BRANCH_ID { get; set; }

        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }

        public Mrs00203Filter() { }
    }
}

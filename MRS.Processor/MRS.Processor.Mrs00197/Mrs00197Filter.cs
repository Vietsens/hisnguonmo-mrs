using MOS.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00197
{
    /// <summary>
    /// Báo cáo thống kê dịch vụ kỹ thuật thanh toán bhyt biểu mẫu 21 tách tuyến thêm nhóm khác (yêu cầu của đan phượng)
    /// </summary>
    public class Mrs00197Filter : FilterBase
    {
        public long BRANCH_ID { get; set; }

        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }

        /// <summary>
        /// True: chi lay noi tru
        /// False: chi lay ngoai tru
        /// Null: lay ca hai
        /// </summary>
        public bool? IS_TREAT { get; set; }

        public Mrs00197Filter()
        {

        }
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00006
{
    /// <summary>
    /// Filter báo cáo thống kê 15 ngày sử dụng
    /// </summary>
    class Mrs00006Filter
    {
        public long? MEDI_STOCK_ID { get;  set;  } //bat buoc co thong tin

        /// <summary>
        /// Thoi gian bat dau lay du lieu bao cao
        /// </summary>
        public long TIME_FROM { get;  set;  } //bat buoc co thong tin

        public long? DEPARTMENT_ID { get; set; }

        public Mrs00006Filter()
            : base()
        {
        }
    }
}

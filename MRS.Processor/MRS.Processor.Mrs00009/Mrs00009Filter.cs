using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00009
{
    /// <summary>
    /// Filter báo cáo sử dụng vật tư
    /// </summary>
    class Mrs00009Filter
    {
        /// <summary>
        /// Thoi gian tong hop du lieu
        /// </summary>
        public long? TIME_FROM { get;  set;  }
        public long? TIME_TO { get; set; }
        public List<long> REQ_DEPARTMENT_IDs { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }

        public Mrs00009Filter()
            : base()
        {
        }
    }
}

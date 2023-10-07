using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00007
{
    /// <summary>
    /// Filter Báo cáo sử dụng thuốc
    /// </summary>
    class Mrs00007Filter
    {
        /// <summary>
        /// Thoi gian tong hop du lieu
        /// </summary>
        public long? TIME_FROM { get;  set;  }
        public long? TIME_TO { get; set; }
        public List<long> REQ_DEPARTMENT_IDs { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public List<long> MEDICINE_TYPE_IDs { get; set; }
        public Mrs00007Filter()
            : base()
        {
        }
    }
}

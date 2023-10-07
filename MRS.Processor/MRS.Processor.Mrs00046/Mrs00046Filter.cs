using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00046
{
    /// <summary>
    /// Báo cáo tổng hợp số lượng các dịch vụ đã thực hiện
    /// </summary>
    class Mrs00046Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public long? DEPARTMENT_ID { get; set; }

        public List<long> SERVICE_TYPE_IDs { get; set; }
        public List<long> SERVICE_IDs { get; set; }

        public Mrs00046Filter()
            : base()
        {
        }
    }
}

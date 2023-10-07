using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00128
{
    /// <summary>
    /// Báo cáo tổng hợp số lượng thuốc trong ngày của 1 loại thuốc
    /// </summary>
    class Mrs00128Filter
    {
        public long MEDICINE_TYPE_ID { get;  set;  }

        public long DATE_FROM { get;  set;  }
        public long DATE_TO { get;  set;  }

        public List<long> BRANCH_IDs { get; set; }
        public long? BRANCH_ID { get; set; }
    }
}

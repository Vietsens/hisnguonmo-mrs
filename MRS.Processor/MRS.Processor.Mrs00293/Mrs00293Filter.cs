using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00293
{
    public class Mrs00293Filter
    {
        public long? TIME_FROM { get;  set;  }
        public long? TIME_TO { get;  set;  }
        public long? BRANCH_ID { get;  set;  }
        public long? SERVICE_TYPE_ID { get; set; }
        public long? SERVICE_ID { get; set; }
        public long? REQUEST_DEPARTMENT_ID { get; set; }
        public long? REQUEST_ROOM_ID { get; set; }
        public List<long> SERVICE_IDs { get; set; }
        public List<long> SERVICE_TYPE_IDs { get; set; }
        public bool? IS_ADD_TREATIN { get; set; } // có thêm thanh toán của bệnh nhân nội trú không?

        public Mrs00293Filter() { }
    }
}

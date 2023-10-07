using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00365
{
    public class Mrs00365Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }
        public long? BRANCH_ID { get; set; }
        public long? REQUEST_DEPARTMENT_ID { get; set; }
        public long SERVICE_TYPE_ID { get;  set;  }

        public Mrs00365Filter() { }

        public bool? IS_WHEN_TREATIN { get; set; }

        public bool? IS_NOT_WHEN_OUT { get; set; }
    }
}

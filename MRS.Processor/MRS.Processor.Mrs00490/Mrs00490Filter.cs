using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00490
{
    public class Mrs00490Filter
    {
        public long? FINISH_TIME_FROM { get; set; }
        public long? FINISH_TIME_TO { get; set; }
        public long? INTRUCTION_TIME_FROM { get; set; }
        public long? INTRUCTION_TIME_TO { get; set; }
        public long? REQUEST_DEPARTMENT_ID { get; set; }
        public long? EXECUTE_DEPARTMENT_ID { get;  set;  }
        public List<long> REQUEST_DEPARTMENT_IDs { get;  set;  }
        public List<long> EXECUTE_DEPARTMENT_IDs { get;  set;  }
        public bool CHECK_PT { get;  set;  }
        public bool CHECK_TT { get;  set;  }
        public string KEY_GROUP_EXPEND { get; set; }
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.Filter; 

namespace MRS.Processor.Mrs00373
{
    public class Mrs00373Filter: FilterBase
    {
        public long? CLINICAL_IN_TIME_FROM { get;  set;  }
        public long? CLINICAL_IN_TIME_TO { get;  set;  }
        public long? FEE_LOCK_TIME_FROM { get;  set;  }
        public long? FEE_LOCK_TIME_TO { get;  set;  }
        public long? REQUEST_DEPARTMENT_ID { get;  set;  }     
    }
}

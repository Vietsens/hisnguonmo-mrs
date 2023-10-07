using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00383
{
    public class Mrs00383Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public long? DEPARTMENT_ID { get;  set;  }
        public long? BRANCH_ID { get;  set;  }
    }
}

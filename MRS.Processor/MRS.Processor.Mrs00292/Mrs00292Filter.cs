using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00292
{
    public class Mrs00292Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public long? BRANCH_ID { get;  set;  }

        public Mrs00292Filter() { }
    }
}

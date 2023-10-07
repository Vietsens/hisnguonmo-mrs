using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00231
{
    public class Mrs00231Filter
    {
        public long FINISH_TIME_FROM { get;  set;  }
        public long FINISH_TIME_TO { get;  set;  }
        public List<long> PTTT_GROUP_IDs { get;  set;  }
        public long DEPARTMENT_ID { get;  set;  }
    }
}

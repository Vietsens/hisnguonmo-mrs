using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00493
{
    public class Mrs00493Filter
    {
        public long? DATE_TIME { get;  set;  }
        public List<long> DEPARTMENT_IDs { get;  set;  }
        public List<long> ROOM_IDs { get;  set;  }
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.Filter; 

namespace MRS.Processor.Mrs00366
{
    public class Mrs00366Filter: FilterBase
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public long SERVICE_TYPE_ID { get;  set;  }
        public long? SERVICE_ID { get;  set;  }
        public long BRANCH_ID { get;  set;  }
        
        
        public Mrs00366Filter()
            : base()
        { }
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00480
{
    public class Mrs00480Filter
    {
        public long TIME_FROM { get;  set;  }                 // thời gian thực hiện giao địch
        public long TIME_TO { get;  set;  }

        public long? BRANCH_ID { get;  set;  }

        public Mrs00480Filter() { }
    }
}

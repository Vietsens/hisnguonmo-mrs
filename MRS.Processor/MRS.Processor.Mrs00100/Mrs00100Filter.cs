using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00100
{
    public class Mrs00100Filter
    {
        public long? BRANCH_ID { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public bool? IS_TREAT { get;  set;  }

        public Mrs00100Filter()
            : base()
        {

        }

        public List<long> BRANCH_IDs { get; set; }
    }
}

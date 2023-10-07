using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00079
{
    public class Mrs00079Filter
    {
        public long? BRANCH_ID { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public long? PATIENT_TYPE_ID { set; get; }

        public List<long> BRANCH_IDs { get; set; }
    }
}

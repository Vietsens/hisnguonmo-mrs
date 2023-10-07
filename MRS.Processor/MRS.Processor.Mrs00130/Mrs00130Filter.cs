using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00130
{
    class Mrs00130Filter
    {
        public long ICD_GROUP_ID { get;  set;  }
        public long DATE_FROM { get;  set;  }
        public long DATE_TO { get;  set;  }
        public long? GENDER_ID { get; set; }
        public long? AGE_FROM { get;  set;  }
        public long? AGE_TO { get;  set;  }
        public List<long> BRANCH_IDs { get; set; }
        public long? BRANCH_ID { get; set; }
    }
}

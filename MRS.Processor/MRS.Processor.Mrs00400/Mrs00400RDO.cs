using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00400
{
    public class Mrs00400RDO
    {
        public string PROVINCE_TYPE { get;  set;  }
        public string PROVINCE_NAME { get;  set;  }
        public long TOTAL_EXAM { get;  set;  }
        public long TOTAL_IN { get;  set;  }
        public long IN_TREATMENT_TIME { get;  set;  }
        public long TOTAL_OUT { get;  set;  }
        public long OUT_TREATMENT_TIME { get;  set;  }
        public long TOTAL_DEATH { get;  set;  }

    }
}

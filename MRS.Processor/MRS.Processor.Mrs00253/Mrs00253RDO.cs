using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 

namespace MRS.Processor.Mrs00253
{
    public class Mrs00253RDO
    {
        public string DEPARTMENT_NAME { get;  set;  }
        public long COUNT_TOTAL { get;  set;  }
        public long COUNT_BHYT { get;  set;  }
        public long COUNT_DV { get;  set;  }
        public long COUNT_KH { get;  set;  }
        public long COUNT_EM { get;  set;  }
        public long COUNT_IN { get;  set;  }
        public long COUNT_TRAN { get;  set;  }
        public long COUNT_DIE { get;  set;  }
        public long COUNT_TOTAL_DT { get;  set;  }
        public double COUNT_TIME { get; set; }

    }
}

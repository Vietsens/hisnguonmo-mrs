using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00234
{
    public class Mrs00234RDO
    {
        public long? REQ_DEPARTMENT_ID { get;  set;  }
        public string REQ_DEPARTMENT_NAME { get;  set;  }
        public string IMP_MEST_CODE { get;  set;  }
        public long IMP_MEST_ID { get;  set;  }
        public Decimal TOTAL_PRICE { get;  set;  }
        public long EXP_MEST_TYPE_ID { get;  set;  }
        public string EXP_MEST_TYPE_NAME { get;  set;  }
        public string IMP_TIME { get;  set;  }

    }
}

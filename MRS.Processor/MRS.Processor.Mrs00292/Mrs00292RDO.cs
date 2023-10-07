using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00292
{
    public class Mrs00292RDO
    {
        public string EXECUTE_ROOM_NAME { get;  set;  }
        public string EXECUTE_ROOM_CODE { get;  set;  }

        public decimal TOTAL_AMOUNT { get;  set;  }
        public decimal TOTAL_BHYT_AMOUNT { get;  set;  }
        public decimal TOTAL_FEE_AMOUNT { get;  set;  }
        public decimal TOTAL_OTHER_AMOUNT { get;  set;  }
        public decimal TOTAL_CC_AMOUNT { get;  set;  }

        public Mrs00292RDO() { }
    }
}

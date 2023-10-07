using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00461
{
    public class Mrs00461RDO
    {
        public string DEPARTMENT_NAME { get;  set;  }
        public string SERVICE_GROUP_NAME { get;  set;  }
        public string SERVICE_NAME { get;  set;  }

        public decimal AMOUNT { get;  set;  }
        public decimal PRICE { get;  set;  }
        public decimal TOTAL_PRICE { get;  set;  }
        public decimal TOTAL_PRICE_BILL { get;  set;  }

        public long SERVICE_GROUP_ID { get;  set;  }
        public long DEPARTMENT_ID { get;  set;  }
        public long SERVICE_ID { get;  set;  }

    }
}

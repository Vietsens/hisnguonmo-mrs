using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00457
{
    public class Mrs00457RDO
    {
        public string DEPARTMENT_NAME { get;  set;  }
        public string SERVICE_TYPE_NAME { get;  set;  }
        public string SERVICE_NAME { get;  set;  }

        public decimal AMOUNT { get;  set;  }
        public decimal PRICE { get;  set;  }
        public decimal TOTAL_PRICE { get;  set;  }
        public decimal TOTAL_PRICE_BILL { get;  set;  }

        public long SERVICE_TYPE_ID { get;  set;  }
        public long DEPARTMENT_ID { get;  set;  }
        public long SERVICE_ID { get;  set;  }

    }
}

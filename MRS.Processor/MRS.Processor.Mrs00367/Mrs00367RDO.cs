using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00367
{
    class Mrs00367RDO
    {
        // báo cáo xuất hủy
        public long NUMBER { get;  set;  }

        public long GROUP_ID { get;  set;  }
        public string GROUP_NAME { get;  set;  }

        public long SERVICE_TYPE_ID { get;  set;  }
        public string SERVICE_TYPE_CODE { get;  set;  }
        public string SERVICE_TYPE_NAME { get;  set;  }

        //public long SERVICE_ID { get;  set;  }

        public string SERVICE_UNIT_NAME { get;  set;  }
        public string MANUFACTURER_NAME { get;  set;  }
        public string NATIONAL_NAME { get;  set;  }

        public decimal AMOUNT { get;  set;  }
        public decimal IMP_PRICE { get;  set;  }
    }
}

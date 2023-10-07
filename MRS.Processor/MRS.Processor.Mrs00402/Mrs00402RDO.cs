using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00402
{
    public class Mrs00402RDO
    {
        // báo cáo tổng hợp sử dụng thuốc viện phí

        public long GROUP_ID { get;  set;  }
        public string GROUP_NAME { get;  set;  }

        public long SERVICE_TYPE_ID { get;  set;  }
        public string SERVICE_TYPE_CODE { get;  set;  }
        public string SERVICE_TYPE_NAME { get;  set;  }

        public long SERVICE_ID { get;  set;  }

        public string ACTIVE_INGR_BHYT_NAME { get; set; }

        public string MANUFACTURER_NAME { get; set; }
        public string NATIONAL_NAME { get;  set;  }
        public string PACKAGE_NUMBER { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }

        public long? EXPIRED_DATE { get;  set;  }
        public decimal AMOUNT { get;  set;  }
        public decimal IMP_PRICE { get; set; }

        public string SUPPLIER_NAME { get; set; }

        public string SUPPLIER_CODE { get; set; }

        public long? SUPPLIER_ID { get; set; }

        public long? IMP_TIME { get; set; }

        public Mrs00402RDO() { }
    }
}

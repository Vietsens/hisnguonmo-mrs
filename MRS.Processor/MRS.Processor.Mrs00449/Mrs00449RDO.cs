using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00449
{
    public class Mrs00449RDO
    {
        // báo cáo kiểm nhập
        public int NUMBER { get;  set;  }

        public long SERVICE_GROUP_ID { get;  set;  }
        public string SERVICE_GROUP_NAME { get;  set;  }

        public long MEDICINE_TYPE_ID { get;  set;  }
        public string MEDICINE_TYPE_NAME { get;  set;  }
        public string MEDICINE_TYPE_CODE { get;  set;  }

        public long MEDICINE_ID { get;  set;  }

        public string SERVICE_UNIT_NAME { get;  set;  }

        public string PACKING_TYPE_NAME { get;  set;  }
        public string NATIONAL_NAME { get;  set;  }
        public string MANUFACTURER_NAME { get;  set;  }

        public string IMP_MEST_CODE { get;  set;  }

        public decimal AMOUNT { get;  set;  }

        public decimal IMP_PRICE { get;  set;  }
        public decimal IMP_VAT_RATIO { get;  set;  }

        public string PACKAGE_NUMBER { get;  set;  }
        public long EXPIRED_DATE { get;  set;  }
    }
}

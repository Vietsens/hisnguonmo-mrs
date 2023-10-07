using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00452
{
    public class Mrs00452RDO
    {
        // báo cáo bán thuốc nhà thuốc

        public V_HIS_EXP_MEST SALE_EXP_MEST { get;  set;  }
        public string SERVICE_TYPE_CODE { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public long SERVICE_ID { get;  set;  }

        public string CONCENTRA { get;  set;  }

        public V_HIS_TRANSACTION BILL { get;  set;  }
        public long NUM_ORDER { get;  set;  }
        public string TRANSACTION_CODE { get;  set;  }

        public decimal AMOUNT { get;  set;  }
        public decimal IMP_PRICE { get;  set;  }
        public decimal PRICE { get;  set;  }
        public decimal VAT_RATIO { get; set; }
        public decimal IMP_VAT_RATIO { get; set; }
        public string PACKAGE_NUMBER { get; set; }
    }
}

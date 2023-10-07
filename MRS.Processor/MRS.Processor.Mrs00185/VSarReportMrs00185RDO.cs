using MRS.MANAGER.Core.MrsReport.RDO.RDOImpExpMestType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00185
{
    class VSarReportMrs00185RDO : IMP_EXP_MEST_TYPE
    {
        public long MEDI_STOCK_ID { get;  set;  }
        public int SERVICE_TYPE_ID { get;  set;  }

        public long MEDI_MATE_ID { get;  set;  }
        public long SERVICE_ID { get;  set;  }

        public string SERVICE_CODE { get;  set;  }
        public string SERVICE_NAME { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }
        public string SERVICE_TYPE_NAME { get;  set;  }

        public decimal BEGIN_AMOUNT { get;  set;  }
        public decimal END_AMOUNT { get;  set;  }
        public decimal IMP_PRICE { get;  set;  }


        public long? SUPPLIER_ID { get;  set;  }
        public string SUPPLIER_CODE { get;  set;  }
        public string SUPPLIER_NAME { get;  set;  }

        public long? BID_ID { get;  set;  }
        public string BID_NUMBER { get;  set;  }

        public long? NUM_ORDER { get;  set;  }

        public string ACTIVE_INGR_BHYT_NAME { get; set; }
        public string CONCENTRA { get; set; }
        public string MEDICINE_USE_FORM_NAME { get; set; }
        public string MANUFACTURER_NAME { get; set; }	
        public string NATIONAL_NAME { get; set; }	
        public string PACKING_TYPE_NAME { get; set; }
        public string EXPIRED_DATE_STR { get; set; }
        public string REGISTER_NUMBER { get; set; }
        public string BID_NAME { get; set; }	
        public decimal BID_AMOUNT { get; set; }
        public decimal BHYT_AMOUNT_BH { get; set; }
        public decimal BHYT_AMOUNT_VP { get; set; }
        public decimal VP_AMOUNT { get; set; }

    }
}

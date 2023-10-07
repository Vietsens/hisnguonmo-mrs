using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00438
{
    public class Mrs00438RDO
    {

        public string IMP_MEST_CODE { get;  set;  }
        public string SERVICE_TYPE_NAME { get;  set;  }
        public string PACKING_TYPE_NAME { get;  set;  }
        public string MANU_NAME { get; set; }
        public string NATIONAL_NAM { get; set; }
        public string MEDI_STOCK_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get;  set;  }
        public string PACKAGE_NUMBER { get;  set;  }
        public long? EXPIRED_DATE{ get;  set;  }
        public decimal IMP_AMOUNT { get;  set;  }
        public decimal PRICE { get;  set;  }
        public string DOCUMENT_NUMBER { get; set; }
        public decimal? VAT_RATIO { get; set; }
       
    }
}

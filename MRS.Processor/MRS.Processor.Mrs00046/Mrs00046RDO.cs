using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00046
{
    class Mrs00046RDO
    {
        public string SERVICE_CODE { get;  set;  }
        public string SERVICE_NAME { get;  set;  }

        public decimal AMOUNT { get;  set;  }

        public long SERVICE_ID { get; set; }

        public decimal EXPEND_PRICE { get; set; }

        public decimal PRICE { get; set; }

        public string SERVICE_TYPE_CODE { get; set; }

        public string SERVICE_TYPE_NAME { get; set; }
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00090
{
    class Mrs00090RDO
    {
        /// <summary>
        /// Theo loại thuốc
        /// </summary>
        public string MEDICINE_TYPE_CODE { get;  set;  }
        public string MEDICINE_TYPE_NAME { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }
        public string CONCENTRA { get;  set;  }

        public long? MEDICINE_TYPE_ID { get;  set;  }
        public decimal IMP_PRICE { get;  set;  }
        public decimal AMOUNT { get;  set;  }
    }
}

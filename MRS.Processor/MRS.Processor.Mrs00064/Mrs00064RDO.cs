using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00064
{
    class Mrs00064RDO
    {
        public long MEDICINE_TYPE_ID { get;  set;  }
        public string MEDICINE_TYPE_CODE { get;  set;  }
        public string MEDICINE_TYPE_NAME { get;  set;  }

        public long MATERIAL_TYPE_ID { get;  set;  }
        public string MATERIAL_TYPE_CODE { get;  set;  }
        public string MATERIAL_TYPE_NAME { get;  set;  }

        public string SERVICE_UNIT_NAME { get;  set;  }
        public string CONCENTRA { get;  set;  }

        public decimal AMOUNT { get;  set;  }
        public decimal PRICE { get; set; }
        public decimal VIR_TOTAL_PRICE { get;  set;  }
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00209
{
    public class Mrs00209RDO
    {
        public string MATERIAL_TYPE_CODE { get;  set;  }
        public string MATERIAL_TYPE_NAME { get;  set;  }
        public Decimal AMOUNT { get;  set;  }
        public Decimal PRICE { get;  set;  }
        public Decimal TOTAL_PRICE { get;  set;  }
        public long TDL_PATIENT_ID { get;  set;  }
        public string VIR_PATIENT_NAME{ get;  set;  }
        public long MATERIAL_ID { get;  set;  }///
    }
}

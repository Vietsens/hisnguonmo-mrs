using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 

namespace MRS.Processor.Mrs00209
{
    public class MATERIAL_VIEW
    {
        public string MATERIAL_TYPE_CODE { get;  set;  }
        public string MATERIAL_TYPE_NAME { get;  set;  }
        public decimal? PRICE { get;  set;  }
        public decimal AMOUNT { get;  set;  }
        public long EXP_MEST_ID { get;  set;  }
        public long MATERIAL_ID { get;  set;  }
    }
}

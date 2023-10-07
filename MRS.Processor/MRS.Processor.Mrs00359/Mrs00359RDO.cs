using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00359
{
    public class Mrs00359RDO
    {
        public long REPORT_TYPE_CAT_ID { get;  set;  }
        public string CATEGORY_NAME { get;  set;  }
        public long MATERIAL_TYPE_ID { get;  set;  }
        public string MATERIAL_TYPE_NAME { get;  set;  }
        public string UNIT { get;  set;  }
        public string IMP_MEST_CODE { get;  set;  }
        public Decimal IMP_PRICE { get;  set;  }
        public Decimal IMP_VAT_RATIO { get;  set;  }
        public Decimal AMOUNT { get;  set;  }
        public Decimal TOTAL_PRICE { get;  set;  }
        public string IMP_TIME { get;  set;  }

    }
}

using MRS.MANAGER.Core.MrsReport.RDO.RDOImpExpMestType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00323
{
    class Mrs00323RDO: IMP_EXP_MEST_TYPE
    {
        public string BLOOD_ABO_CODE { get;  set;  }

        public long BLOOD_TYPE_ID { get;  set;  }
        public string BLOOD_TYPE_CODE { get;  set;  }
        public string BLOOD_TYPE_NAME { get;  set;  }

        public long BLOOD_ID { get;  set;  }

        public decimal IMP_PRICE { get;  set;  }

        public string SERVICE_UNIT_NAME { get;  set;  }
        public decimal VOLUME { get;  set;  }

        public decimal BEGIN_AMOUNT { get;  set;  }
        public decimal END_AMOUNT { get;  set;  }

    }
}

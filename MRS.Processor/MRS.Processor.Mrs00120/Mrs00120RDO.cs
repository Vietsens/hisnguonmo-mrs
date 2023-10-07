using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00120
{
    class Mrs00120RDO
    {
        public long WORK_PLACE_ID { get;  set;  }
        public string WORK_PLACE_CODE { get;  set;  }
        public string WORK_PLACE_NAME { get;  set;  }

        public long MILITARY_RANK_ID { get;  set;  }
        public string MILITARY_RANK_CODE { get;  set;  }
        public string MILITARY_RANK_NAME { get;  set;  }

        public decimal AMOUNT_GENERAL { get;  set;  }
        public decimal AMOUNT_3AND4SLASH { get;  set;  }
        public decimal AMOUNT_1AND2SLASH { get;  set;  }
        public decimal AMOUNT_LIEUTENANT { get;  set;  }
        public decimal AMOUNT_HSQCS { get;  set;  }
    }
}

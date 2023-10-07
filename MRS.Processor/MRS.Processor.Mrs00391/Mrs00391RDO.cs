using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 
using System.Reflection; 

namespace MRS.Processor.Mrs00391
{
    public class Mrs00391RDO
    {
        public long REQUEST_DEPARTMENT_ID { get;  set;  }// khoa yêu cầu
        public string REQUEST_DEPARTMENT_NAME { get;  set;  }

        public decimal BN_AMOUNT1 { get;  set;  }
        public decimal BN_AMOUNT2 { get;  set;  }
        public decimal BN_AMOUNT3 { get;  set;  }
        public decimal BN_AMOUNT4 { get;  set;  }
        public decimal BN_AMOUNT5 { get;  set;  }
        public decimal BN_AMOUNT6 { get;  set;  }
        public decimal BN_AMOUNT7 { get;  set;  }
        public decimal BN_AMOUNT8 { get;  set;  }
        public decimal XN_AMOUNT1 { get;  set;  }
        public decimal XN_AMOUNT2 { get;  set;  }
        public decimal XN_AMOUNT3 { get;  set;  }
        public decimal XN_AMOUNT4 { get;  set;  }
        public decimal XN_AMOUNT5 { get;  set;  }
        public decimal XN_AMOUNT6 { get;  set;  }
        public decimal XN_AMOUNT7 { get;  set;  }
        public decimal XN_AMOUNT8 { get;  set;  }

    }
}

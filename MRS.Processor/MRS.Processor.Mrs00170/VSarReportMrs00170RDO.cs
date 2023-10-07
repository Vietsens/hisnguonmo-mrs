using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00170
{
    public class VSarReportMrs00170RDO
    {
        
        public string MEDICINE_TYPE_NAME { get;  set;  }
        public string IMP_DATE { get;  set;  }
        public decimal BID_AMOUNT { get;  set;  }
        public decimal BID_INVENTORY { get;  set;  }
        public string IMP_MEST_CODE { get;  set;  }
        public decimal IMP_AMOUNT { get;  set;  }
        public decimal EXP_AMOUNT { get;  set;  }
        public decimal BID_END_AMOUNT { get;  set;  }
    }

    //public class IMP
    //{
    //    public long MEDICINE_ID { get;  set;  }
    //    public string IMP_TIME { get;  set;  }
    //    public string IMP_MEST_CODE { get;  set;  }
    //    public decimal IMP_AMOUNT { get;  set;  }
    //}

    //public class EXP
    //{
    //    public long MEDICINE_ID { get;  set;  }
    //    public decimal? EXP_AMOUNT { get;  set;  }
    //    public decimal? INVENTORY { get;  set;  }
    //}
}

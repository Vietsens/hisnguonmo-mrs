using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00433
{
    public class Mrs00433RDO
    {
        public string TREATMENT_TYPE_NAME { get;  set;  }

        public long TOTAL_PATIENT { get;  set;  }
        public long TOTAL_TREATMENT_TIME { get;  set;  }
        public decimal TOTAL_PRICE_XN { get;  set;  }
        public decimal TOTAL_PRICE_CDHA { get;  set;  }
        public decimal TOTAL_PRICE_THUOC { get;  set;  }
        public decimal TOTAL_PRICE_MAU { get;  set;  }
        public decimal TOTAL_PRICE_PTTT { get;  set;  }
        public decimal TOTAL_PRICE_VTYT { get;  set;  }
        public decimal TOTAL_PRICE_GIUONG { get;  set;  }
        public decimal TOTAL_PRICE_KHAM { get;  set;  }
        public decimal TOTAL_PRICE { get;  set;  }
        public decimal TOTAL_PATIENT_PRICE { get;  set;  }
        public decimal TOTAL_HEIN_PRICE { get;  set;  }

        public decimal TOTAL_PRICE_TRUXQ { get;  set;  }

    }
}

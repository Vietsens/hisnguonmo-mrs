using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 
using System.Reflection; 

namespace MRS.Processor.Mrs00474
{
    public class Mrs00474RDO
    {
        public string DEPARTMENT_NAME { get;  set;  }	
        public long TREATMENT_COUNT { get;  set;  }	
        public Decimal? TOTAL_PRICE { get;  set;  }	
        public Decimal? HEIN_TOTAL_PRICE { get;  set;  }
        public Decimal? PATIENT_TOTAL_PRICE { get;  set;  }
        public Decimal? PATIENT_TOTAL_PRICE_BHYT { get;  set;  }
        public Decimal? FEE_TOTAL_PRICE { get;  set;  }
        public REVENUE revenue { get;  set;  }
        public REVENUE revenuePk { get;  set;  }
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00450
{
    public class Mrs00450RDO
    {
        public string MEDICINE_TYPE_CODE { get;  set;  }
        public string MEDICINE_TYPE_NAME { get;  set;  }
        public string ACTIVE_INGR_BHYT_NAME { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }
        public decimal? EXP_PRICE { get;  set;  }
        public decimal? IMP_PRICE { get; set; }
        
        
    }
}

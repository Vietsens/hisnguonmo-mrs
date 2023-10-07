using MOS.EFMODEL.DataModels; 
using MOS.MANAGER.HisTreatment; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00467
{
    public class Mrs00467RDO
    {
        public long MEDICINE_ID { get;  set;  }
        public long MEDICINE_TYPE_ID { get;  set;  }
        public string MEDICINE_TYPE_CODE { get;  set;  }
        public string MEDICINE_TYPE_NAME { get;  set;  }
        public string MANUFACTURER_NAME { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }
        public decimal BEGIN_AMOUNT { get;  set;  }
        public decimal IMP_AMOUNT { get;  set;  }
        public decimal IMP_PRICE { get;  set;  }
        public decimal EXP_AMOUNT { get;  set;  }
        public decimal DELETE_AMOUNT { get;  set;  }
        public decimal FINISH_AMOUNT { get;  set;  }
        
        public Mrs00467RDO() { }
    }
}

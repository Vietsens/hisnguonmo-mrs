using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00432
{
    public class Mrs00432RDO
    {
        public V_HIS_BLOOD_TYPE BLOOD_TYPE { get;  set;  }

        public long BLOOD_ID { get;  set;  }
        public string BLOOD_CODE { get;  set;  }

        public long? IMP_EXP_TIME { get;  set;  }
        public string IMP_MEST_CODE { get;  set;  }
        public string EXP_MEST_CODE { get;  set;  }
        public string IMP_EXP_MEST_CODE { get;  set;  }
        public string BLOOD_ABO_CODE { get;  set;  }

        public string NOTE { get;  set;  }

        public decimal BEGIN_AMOUNT { get;  set;  }

        public decimal IMP_AMOUNT { get;  set;  }
        public decimal EXP_AMOUNT { get;  set;  }

        public decimal END_AMOUNT { get;  set;  }
    }
}

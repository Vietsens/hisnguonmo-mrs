using MOS.EFMODEL.DataModels; 
using MOS.MANAGER.HisTreatment; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00419
{
    public class Mrs00419RDO
    {
        public long? IMP_EXP_TIME { get;  set;  }
        public long? EXPIRED_DATE { get;  set;  }
        public string IMP_EXP_MEST_CODE { get;  set;  }
        public string PACKAGE_NUMBER { get;  set;  }
        public string TREATMENT_CODE { get;  set;  }
        public string PATIENT_NAME { get;  set;  }
        public decimal? IMP_AMOUNT { get;  set;  }
        public decimal? EXP_AMOUNT { get;  set;  }

        public Mrs00419RDO() { }
    }
}

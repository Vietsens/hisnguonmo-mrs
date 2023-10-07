using MOS.MANAGER.HisService;
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 
using Inventec.Common.Logging; 
using MOS.Filter; 
using MOS.MANAGER.HisIcd; 
using MRS.MANAGER.Config; 
using MOS.MANAGER.HisServiceReq; 
using MOS.MANAGER.HisSereServ; 

namespace MRS.Processor.Mrs00259
{
    public class Mrs00259RDO
    {
        public long BLOOD_TYPE_ID { get;  set;  }
        public string BLOOD_TYPE_NAME { get; set; }
        public string BLOOD_TYPE_CODE { get; set; }
        public string BLOOD_CODE { get; set; }

        public decimal VOLUME { get; set; }
        public string BLOOD_ABO_CODE { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public decimal IMP_PRICE { get;  set;  }

        public decimal BEGIN_AMOUNT { get;  set;  }

        public decimal IMP_AMOUNT { get;  set;  }
        public decimal IMP_MANU { get;  set;  }
        public decimal IMP_CHMS { get;  set;  }
        public decimal IMP_OTHER { get;  set;  }
        public decimal IMP_MOBA_PRES { get;  set;  }

        public decimal EXP_AMOUNT { get;  set;  }
        public decimal EXP_PRES { get;  set;  }
        public decimal EXP_CHMS { get;  set;  }
        public decimal EXP_OTHER { get;  set;  }
        public decimal EXP_MANU { get;  set;  }

        public decimal END_AMOUNT { get;  set;  }

    }
}

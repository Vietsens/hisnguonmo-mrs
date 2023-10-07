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

namespace MRS.Processor.Mrs00258
{
    public class Mrs00258RDO
    {
        public V_HIS_MEDI_STOCK MEDI_STOCK { get;  set;  }

        public int SERVICE_GROUP_ID { get;  set;  }
        public string SERVICE_GROUP_NAME { get;  set;  }

        public long SERVICE_TYPE_ID { get;  set;  }
        public string SERVICE_TYPE_CODE { get;  set;  }
        public string SERVICE_TYPE_NAME { get;  set;  }

        public long SERVICE_ID { get;  set;  }

        public long PACKAGE_NUMBER { get;  set;  }

        public string SERVICE_UNIT_NAME { get;  set;  }

        public string SUPPLIER_NAME { get;  set;  }

        public string MANUFACTURER_NAME { get;  set;  }

        public decimal IMP_PRICE { get;  set;  }

        public decimal AMOUNT { get;  set;  }
    }
}

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

namespace MRS.Processor.Mrs00260
{
    public class Mrs00260RDO : V_HIS_MEDICINE_TYPE
    {
        public string ACTIVE_INGREDIENT_CODE { get;  set;  }
        public string ACTIVE_INGREDIENT_NAME { get;  set;  }
        public Decimal? AMOUNT_MEDICINE { get;  set;  }
        public string MANUFACTURE_NAME { get;  set;  }
        public string SUPPLIER_NAME { get;  set;  }


        public string PACKAGE_NUMBER { get; set; }

        public string EXPIRED_DATE_STR { get; set; }
    }
}

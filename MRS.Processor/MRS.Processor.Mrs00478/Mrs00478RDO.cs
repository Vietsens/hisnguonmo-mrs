using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00478
{
    public class Mrs00478RDO
    {
        // xuất thuốc bn
        public string GROUP_SERVICE { get;  set;  }

        public string SERVICE_TYPE_CODE { get;  set;  }
        public string SERVICE_TYPE_NAME { get;  set;  }

        public string SERVICE_UNIT_NAME { get;  set;  }
        public string NATIONAL_NAME { get;  set;  }
        public decimal AMOUNT { get;  set;  }

        public Mrs00478RDO() { }
    }
}

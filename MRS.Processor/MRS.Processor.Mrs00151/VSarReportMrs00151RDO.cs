using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00151
{
    public class VSarReportMrs00151RDO
    {
        public int NUMBER_ORDER { get;  set;  }

        public string MEDICINE_TYPE_NAME { get;  internal set;  }

        public string SERVICE_UNIT_NAME { get;  internal set;  }

        public string NATIONAL_NAME { get;  internal set;  }

        public decimal? PRICE { get;  internal set;  }

        public decimal? AMOUNT { get;  internal set;  }

        public decimal? TOTAL_PRICE { get;  internal set;  }
    }
}

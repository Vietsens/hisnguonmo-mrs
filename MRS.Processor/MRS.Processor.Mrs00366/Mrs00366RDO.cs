using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00366
{
    public class Mrs00366RDO
    {
        public string SERVICE_NAME { get;  set;  }                     
        public decimal? AMOUNT { get;  set;  }
        public decimal? PRICE { get;  set;  }
        public decimal? HEIN_PRICE { get;  set;  }
        public decimal? TOTAL_PRICE { get;  set;  }
        public decimal? DCT_BH { get;  set;  }
        public decimal? DCT_BN { get;  set;  }
    }
}

using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00258
{
   
    public class Mrs00258Filter
    {
        public long CREATE_TIME { get;  set;  }

        public List<long> MEDI_STOCK_IDs { get;  set;  }

        public bool IS_MATERIAL { get;  set;  }
        public bool IS_MEDICINE { get;  set;  }
        public bool IS_CHEMICAL { get;  set;  }
        public bool IS_BLOOD { get;  set;  }
    }
}

using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00158
{
    public class Mrs00158Filter : FilterBase
    {
        public List<long> MEDI_STOCK_IDs { get;  set;  }
        public List<long> IMP_MEDI_STOCK_IDs { get;  set;  }
        public long EXP_TIME_FROM { get;  set;  }
        public long EXP_TIME_TO { get;  set;  }
        public bool? IS_MEDICINE { get;  set;  }
        public bool? IS_MATERIAL { get;  set;  }
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00449
{
    public class Mrs00449Filter
    {
        public long TIME_FROM { get;  set;  }         // thời gian nhập kho
        public long TIME_TO { get;  set;  }

        public long MEDI_STOCK_ID { get;  set;  }     // kho nhập
        public long IMP_SOURCE_ID { get;  set;  }     // nguồn nhập

        public bool IS_NEUROLOGICAL { get;  set;  }   // là thuốc hướng thần
        public bool IS_ADDICTIVE { get;  set;  }      // là thuốc gây nghiện
        public bool IS_MEDICINE { get;  set;  }       // là thuốc thường
    }
}

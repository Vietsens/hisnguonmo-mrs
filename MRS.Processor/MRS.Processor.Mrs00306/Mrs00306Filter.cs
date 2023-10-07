using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00306
{
    public class Mrs00306Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public List<string> LOGINNAMEs { get;  set;  }
        public string LOGINNAME { get;  set;  }
        
        public List<long> MEDI_STOCK_BUSINESS_IDs { get;  set;  }
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00394
{
    public class Mrs00394RDO
    {
        public long EXP_TIME { get;  set;  }
        public long EXP_DATE { get;  set;  }
        public string EXP_DATE_STR { get;  set;  }

        public decimal AMOUNT { get;  set;  }

        public Mrs00394RDO() { }
    }
}

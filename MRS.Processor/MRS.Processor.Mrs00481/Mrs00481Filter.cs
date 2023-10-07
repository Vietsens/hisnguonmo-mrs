using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00481
{
    public class Mrs00481Filter
    {
        public long TIME_FROM { get;  set;  }                 // thời gian thực hiện giao địch
        public long TIME_TO { get;  set;  }

        public long? BRANCH_ID { get;  set;  }

        public bool IS_BILL { get;  set;  }                   // thanh toán
        public bool IS_DEPOSIT { get;  set;  }                // tạm ứng

        public Mrs00481Filter() { }
    }
}

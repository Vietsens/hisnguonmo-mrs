using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00263
{
    public class Mrs00263Filter : FilterBase
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public long? BRANCH_ID { get;  set;  }
        public long? PAY_FORM_ID { get; set; }
        public string CASHIER_LOGINNAME { get; set; }
        public string LOGINNAME { get; set; }
    }
}

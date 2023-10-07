using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00336
{
    public class Mrs00336Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public List<long> ACCOUNT_BOOK_IDs { get; set; }
        public List<string> CASHIER_LOGINNAMEs { get; set; }
        public List<string> LOGINNAMEs { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public bool? IS_RP_OTHER_DATE { get; set; }
    }
}

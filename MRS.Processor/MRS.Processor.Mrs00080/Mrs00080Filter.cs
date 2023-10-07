using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00080
{
    /// <summary>
    /// De nghi thanh toan bao hiem y te benh nhan kham chua benh noi tru va ngoai tru: File Mem 46 cot
    /// </summary>
    class Mrs00080Filter
    {
        public long? BRANCH_ID { get; set; }
        public bool? IS_MERGE_TREATMENT { get; set; }

        public long? TIME_FROM { get;  set;  }
        public long? TIME_TO { get;  set;  }

        public Mrs00080Filter()
            : base()
        {
        }

        public List<long> BRANCH_IDs { get; set; }

        public List<string> ICD_CODEs { get; set; }
    }
}

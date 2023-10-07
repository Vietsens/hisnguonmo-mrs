using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00077
{
    /// <summary>
    /// BHYT 19 - QD 917 File mem
    /// </summary>
    class Mrs00077Filter
    {
        public long? BRANCH_ID { get;  set;  }

        public long? TIME_FROM { get;  set;  }
        public long? TIME_TO { get;  set;  }

        public long? PATIENT_TYPE_ID { set; get; }
        public Mrs00077Filter()
            : base()
        {
        }

        public List<long> BRANCH_IDs { get; set; }
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00057
{
    /// <summary>
    /// Bao cao tong hop chi phi goc cua cac dich vu xet nghiem
    /// </summary>
   public class Mrs00057Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public long? PATIENT_TYPE_ID { get;  set;  }
        public List<long> PATIENT_TYPE_IDs { get;  set;  }
        public bool? IS_NOT_REQUIRE_BILL { get; set; }
        public List<long> BRANCH_IDs { get; set; }


        public Mrs00057Filter()
            : base()
        {
        }

        public long? INPUT_DATA_ID_TIME_TYPE { get; set; }
    }
}

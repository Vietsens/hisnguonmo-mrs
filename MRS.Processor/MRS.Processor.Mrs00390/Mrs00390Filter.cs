using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00390
{
    public class Mrs00390Filter 
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public long? PATIENT_TYPE_ID { get;  set;  }
        public List<long> TREATMENT_TYPE_IDs { get;  set;  }
    }
}

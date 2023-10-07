using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00344
{
    public class Mrs00344Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public long? PATIENT_TYPE_ID { get;  set;  }
        public long? SERE_SERV_PATIENT_TYPE_ID { get;  set;  }
    }
}

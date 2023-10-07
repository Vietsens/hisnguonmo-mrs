using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00465
{
    public class Mrs00465Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public long? SERVICE_ID { get;  set;  }
        public List<long> REQUEST_DEPARTMENT_IDs { get;  set;  }

        public long? INPUT_DATA_ID_TIME_TYPE { get; set; }
    }
}

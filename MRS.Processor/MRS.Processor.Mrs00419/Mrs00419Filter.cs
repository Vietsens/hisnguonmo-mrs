using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00419
{
    public class Mrs00419Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public long DEPARTMENT_ID { get;  set;  }
        public long? MEDICINE_TYPE_ID { get;  set;  }
        public long? MATERIAL_TYPE_ID { get;  set;  }
    }
}
	
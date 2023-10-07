using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00500
{
    public class Mrs00500Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public long DEPARTMENT_ID { get;  set;  }

        public bool IS_MEDI_MATE { get;  set;  }
        public bool IS_DIIM { get;  set;  }
        public bool IS_MISU { get;  set;  }
        public bool IS_SURG { get;  set;  }
        public bool IS_EXAM { get;  set;  }
        public bool IS_TEST { get;  set;  }
    }
}

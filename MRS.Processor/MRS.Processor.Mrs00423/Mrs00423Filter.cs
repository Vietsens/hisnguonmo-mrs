using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00423
{
    public class Mrs00423Filter : FilterBase
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public List<string> DOCTOR_LOGINNAMEs { get;  set;  }
        public string DOCTOR_LOGINNAME { get;  set;  }
        public long? DEPARTMENT_ID { get;  set;  }

        public Mrs00423Filter()
            : base()
        {

        }
    }
}

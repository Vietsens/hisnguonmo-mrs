using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00417
{

    public class Mrs00417Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public bool IS_MEDICINE { get;  set;  }
        public bool IS_MATERIAL { get;  set;  }
        public long? DEPARTMENT_ID { get;  set;  }
        public Mrs00417Filter()
            : base()
        {

        }
    }
}

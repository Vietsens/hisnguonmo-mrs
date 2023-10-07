using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00044
{
    /// <summary>
    /// So lu tru ho so benh an
    /// </summary>
    class Mrs00044Filter
    {
        public long? CREATE_TIME_FROM { get;  set;  }
        public long? CREATE_TIME_TO { get;  set;  }

        public long? DEPARTMENT_ID { get;  set;  }

        public Mrs00044Filter()
            : base()
        {
        }
    }
}

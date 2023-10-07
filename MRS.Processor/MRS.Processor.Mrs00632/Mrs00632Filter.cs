using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00632
{
    /// <summary>
    /// So lu tru ho so benh an
    /// </summary>
    class Mrs00632Filter
    {
        public long? TIME_FROM { get;  set;  }
        public long? TIME_TO { get;  set;  }

        public List<long> DEPARTMENT_IDs { get;  set;  }

        public Mrs00632Filter()
            : base()
        {
        }
    }
}

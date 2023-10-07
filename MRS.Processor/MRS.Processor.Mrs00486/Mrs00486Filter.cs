using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00486
{
    /// <summary>
    /// Bao cao tong hop doanh thu theo dich vu
    /// </summary>
    class Mrs00486Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public List<long> REQUEST_DEPARTMENT_IDs { get;  set;  }
        public List<long> EXECUTE_DEPARTMENT_IDs { get;  set;  }

        public Mrs00486Filter()
            : base()
        {
        }
    }
}

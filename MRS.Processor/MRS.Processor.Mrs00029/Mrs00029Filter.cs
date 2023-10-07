using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00029
{
    /// <summary>
    /// Bao cao tong hop doanh thu theo khoa chi dinh
    /// </summary>
    class Mrs00029Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00029Filter()
            : base()
        {
        }
    }
}

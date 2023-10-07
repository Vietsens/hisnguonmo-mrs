using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00028
{
    /// <summary>
    /// Bao cao tong hop doanh thu theo phong chi dinh
    /// </summary>
    class Mrs00028Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00028Filter()
            : base()
        {
        }
    }
}

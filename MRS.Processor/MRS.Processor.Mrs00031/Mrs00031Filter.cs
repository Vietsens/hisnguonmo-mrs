using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00031
{
    /// <summary>
    /// Bao cao tong hop doanh thu dich vu theo nguoi xu ly
    /// </summary>
    class Mrs00031Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00031Filter()
            : base()
        {
        }
    }
}

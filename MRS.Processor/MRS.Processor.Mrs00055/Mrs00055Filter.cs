using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00055
{
    /// <summary>
    /// Bao cao chi tiet doanh thu theo phieu thu thanh toan
    /// </summary>
    class Mrs00055Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00055Filter()
            : base()
        {
        }
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00025
{
    /// <summary>
    /// Bao cao tong hop doanh thu theo doi tuong thanh toan
    /// </summary>
    class Mrs00025Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00025Filter()
            : base()
        {
        }
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00056
{
    /// <summary>
    /// Bao cao chi tiet doanh thu theo hoa don gia tri gia tang
    /// </summary>
    class Mrs00056Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00056Filter()
            : base()
        {
        }
    }
}

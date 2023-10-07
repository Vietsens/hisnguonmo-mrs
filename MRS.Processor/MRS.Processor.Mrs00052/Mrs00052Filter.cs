using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00052
{
    /// <summary>
    /// Bao cao tong hop su dung quyen thu chi
    /// </summary>
    class Mrs00052Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00052Filter()
            : base()
        {
        }
    }
}

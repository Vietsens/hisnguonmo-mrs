using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00060
{
    /// <summary>
    /// Bao cao tong hop doanh thu theo dich vu - doi tuong thanh toan bhyt
    /// </summary>
    class Mrs00060Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public long? BRANCH_ID { get;  set;  }

        public Mrs00060Filter()
            : base()
        {
        }
    }
}

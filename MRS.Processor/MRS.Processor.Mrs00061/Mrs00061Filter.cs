using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00061
{
    /// <summary>
    /// bao cao tong hop doanh thu theo dich vu - doi tuong thanh toan vien phi
    /// </summary>
    class Mrs00061Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public long? BRANCH_ID { get;  set;  }

        public Mrs00061Filter()
            : base()
        {
        }
    }
}

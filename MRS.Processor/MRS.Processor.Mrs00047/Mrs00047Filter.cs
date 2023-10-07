using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00047
{
    /// <summary>
    /// bao cao danh sach benh nhan da ra vien ke toan chua duyet
    /// </summary>
    class Mrs00047Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public long? DEPARTMENT_ID { get;  set;  }

        public Mrs00047Filter()
            : base()
        {
        }
    }
}

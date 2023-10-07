using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00141
{
    /// <summary>
    /// Báo cáo thang theo khoa phan loai theo doi tuong quan doi
    /// </summary>
    class Mrs00141Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public long DEPARTMENT_ID { get;  set;  }
    }
}

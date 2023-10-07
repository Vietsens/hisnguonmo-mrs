using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00118
{
    /// <summary>
    /// Báo cáo thu dung điều trị
    /// </summary>
    class Mrs00118Filter
    {
        public long DEPARTMENT_ID { get;  set;  }

        public long DATE_FROM { get;  set;  }
        public long DATE_TO { get;  set;  }
    }
}

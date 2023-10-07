using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00116
{
    /// <summary>
    /// Báo cáo chi tiết bệnh nhân vào điều trị tại các khoa
    /// </summary>
    class Mrs00116Filter
    {
        public List<long> DEPARTMENT_IDs { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
    }
}

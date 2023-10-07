using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00134
{
    /// <summary>
    /// Báo cáo công tác khám chữa bệnh tháng
    /// </summary>
    class Mrs00134Filter
    {
        public long DEPARTMENT_ID { get;  set;  }

        public long DATE_FROM { get;  set;  }
        public long DATE_TO { get;  set;  }
    }
}

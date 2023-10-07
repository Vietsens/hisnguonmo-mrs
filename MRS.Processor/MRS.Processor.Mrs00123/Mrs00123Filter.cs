using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00123
{
    /// <summary>
    /// Báo cáo thống kê số lượng hồ sơ khám và điều trị theo từng khoa
    /// </summary>
    class Mrs00123Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00123Filter()
            : base()
        {
        }
    }
}

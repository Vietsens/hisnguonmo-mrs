using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00070
{
    /// <summary>
    /// Báo cáo tổng hợp doanh thu theo đối tượng bệnh nhân theo khoa
    /// </summary>
    class Mrs00070Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public long DEPARTMENT_ID { get;  set;  }

        public Mrs00070Filter()
            : base()
        {
        }
    }
}

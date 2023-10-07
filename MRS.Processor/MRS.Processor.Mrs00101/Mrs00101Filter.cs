using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00101
{
    /// <summary>
    /// Báo cáo danh sách bệnh nhân bhyt ngoại trú đề nghị thanh toán c79a theo khoa
    /// </summary>
    class Mrs00101Filter
    {
        public long BRANCH_ID { get;  set;  }
        public long? DEPARTMENT_ID { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public string KEY_SPLIT_SR { get; set; }

        public Mrs00101Filter()
            : base()
        {
        }
    }
}

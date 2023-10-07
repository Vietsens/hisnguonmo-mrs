using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00084
{
    /// <summary>
    /// Báo cáo quyển sổ chi tiết bệnh nhân theo loại đối tượng bhyt (ví dụ: có mã bảo hiểm CA5 (Công an))
    /// </summary>
    class Mrs00084Filter
    {
        public string KEY_HEIN_CARD { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00084Filter()
            : base()
        {
        }
    }
}

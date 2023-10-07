using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00093
{
    /// <summary>
    /// Phiếu công khai thuốc của bệnh nhân
    /// </summary>
    class Mrs00093Filter
    {
        // bắt buộc truyền vào hồ sơ điều trị nếu không truyen vào => id=0=> không có dữ liệu
        public string TREATMENT_CODE { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
    }
}

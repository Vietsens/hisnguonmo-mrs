using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00243
{
    /// <summary>
    /// Báo cáo tổng hợp đề nghị thanh toán bệnh nhân ngoại trú bhyt C79a - loại đối tượng 01 (Sử dụng thẻ cấu hình để 
    /// cấu hình loại thẻ, vd: TQ497,DN497... (Cấu hình HeinCardNumber__HeinType__01); 
    /// </summary>
    public class Mrs00243Filter
    {
        public long BRANCH_ID { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }
        public long? DEPARTMENT_ID { get; set; }

        public Mrs00243Filter() { }
    }
}

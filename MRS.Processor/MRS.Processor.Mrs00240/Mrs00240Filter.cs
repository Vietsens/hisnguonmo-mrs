using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00240
{
    /// <summary>
    /// Báo cáo File mềm danh sách bệnh nhân nội trú đề nghị thanh toán bhyt C80a - loại đối tượng 01 (Sử dụng thẻ cấu hình để 
    /// cấu hình loại thẻ, vd: TQ497,DN497... (Cấu hình HeinCardNumber__HeinType__01); 
    /// </summary>
    public class Mrs00240Filter
    {
        public long BRANCH_ID { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }
        public long? DEPARTMENT_ID { get; set; }

        public Mrs00240Filter() { }
    }
}

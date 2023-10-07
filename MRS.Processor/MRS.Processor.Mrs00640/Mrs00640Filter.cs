using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00640
{
    /// <summary>
    /// Báo cáo File mềm danh sách bệnh nhân nội trú đề nghị thanh toán bhyt C80a - loại đối tượng 03 (Sử dụng thẻ cấu hình để 
    /// cấu hình loại thẻ, Trừ các đối tượng trong 2 thẻ cấu hình HeinCardNumber__HeinType__01, HeinCardNumber__HeinType__02); 
    /// </summary>
    public class Mrs00640Filter
    {
        public long? BRANCH_ID { get; set; }
        public List<long> BRANCH_IDs { get; set; }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00640Filter() { }
    }
}
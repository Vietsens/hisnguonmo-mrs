using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00239
{
    /// <summary>
    /// Báo cáo File mềm danh sách bệnh nhân thanh toán bhyt ngoại trú C79a - loại đối tượng 03 (Sử dụng thẻ cấu hình để cấu 
    ///  hình loại thẻ, Trừ các đối tượng trong 2 thẻ cấu hình HeinCardNumber__HeinType__01, HeinCardNumber__HeinType__02); 
    /// </summary>
    public class Mrs00239Filter
    {
        public long BRANCH_ID { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public bool? IS_MERGE_TREATMENT { get; set; }

        public Mrs00239Filter() { }
    }
}

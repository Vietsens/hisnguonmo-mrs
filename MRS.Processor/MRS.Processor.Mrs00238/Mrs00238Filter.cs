using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00238
{
    /// <summary>
    /// Báo cáo File mềm danh sách bệnh nhân thanh toán bhyt ngoại trú C79a - loại đối tượng 02 (Sử dụng thẻ cấu hình để cấu 
    /// hình thẻ, vd: QN497,... (Cấu hình HeinCardNumber__HeinType__02); 
    /// </summary>
    public class Mrs00238Filter
    {
        public long BRANCH_ID { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }
        public long? DEPARTMENT_ID { get; set; }

        public Mrs00238Filter()
        {

        }
    }
}

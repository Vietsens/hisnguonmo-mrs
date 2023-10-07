using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00210
{
    /// <summary>
    /// Báo cáo đề nghị thánh toán bhyt bệnh nhân kcb ngoại trú C79a theo đối tượng thẻ (Sử dụng thẻ cấu hình để cấu hình thẻ, 
    /// vd: TQ497,DN497... (Cấu hình HeinCardNumber__HeinType__01); 
    /// </summary>
    public class Mrs00210Filter
    {
        public long BRANCH_ID { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public long? DEPARTMENT_ID { get; set; }

        public Mrs00210Filter() { }
    }
}

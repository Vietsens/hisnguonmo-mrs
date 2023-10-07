using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 

namespace MRS.Processor.Mrs00211
{
    /// <summary>
    /// Báo cáo đề nghị thánh toán bhyt bệnh nhân kcb ngoại trú C79a theo đối tượng thẻ (Sử dụng thẻ cấu hình để cấu hình thẻ, 
    /// vd: QN597... (Cấu hình HeinCardNumber__HeinType__02); 
    /// </summary>
    public class Mrs00211Filter
    {
        public long BRANCH_ID { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public long? DEPARTMENT_ID { get; set; }

        public Mrs00211Filter() { }
    }
}

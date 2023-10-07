using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00329
{
    /// <summary>
    /// Báo cáo đề nghị thánh toán bhyt bệnh nhân kcb ngoại trú C79a theo đối tượng thẻ (Sử dụng thẻ cấu hình để cấu hình thẻ, 
    /// vd: TQ497,DN497... (Cấu hình HeinCardNumber__HeinType__01); 
    /// </summary>
    public class Mrs00329Filter
    {
        public long BRANCH_ID { get;  set;  }
        
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public string FEE_LOCK_LOGINNAME { get; set; }

        public bool? IS_PAY { get; set; }

        public short? STATUS_TREATMENT { get; set; }

        public long? END_DEPARTMENT_ID { get; set; }
        /// <summary>
        /// True: chi lay noi tru
        /// False: chi lay ngoai tru
        /// Null: lay ca hai
        /// </summary>
        public bool? IS_TREAT { get; set; }

        public Mrs00329Filter() { }
    }
}

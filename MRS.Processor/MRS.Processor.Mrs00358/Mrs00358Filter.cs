using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00358
{
    /// <summary>
    /// Báo cáo thống kê vật tư thanh toán BHYT 19 theo đối tượng thẻ (Sử dụng thẻ cấu hình để cấu hình thẻ, vd: TQ497,DN497... )
    /// (Cấu hình HeinCardNumber__HeinType__01); 
    /// </summary>
    public class Mrs00358Filter
    {
        public long BRANCH_ID { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        /// <summary>
        /// True: chi lay noi tru
        /// False: chi lay ngoai tru
        /// Null: lay ca hai
        /// </summary>
        public bool? IS_TREAT { get;  set;  }

        public Mrs00358Filter() { }
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00220
{
    /// <summary>
    /// Báo cáo thống kê thuốc thanh toán BHYT 20 theo đối tượng thẻ (Sử dụng thẻ cấu hình để cấu hình thẻ, vd: QN597... )
    /// (Cấu hình HeinCardNumber__HeinType__02); 
    /// </summary>
    public class Mrs00220Filter
    {
        public long BRANCH_ID { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00220Filter() { }
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00218
{
    /// <summary>
    /// Báo cáo thống kê vật tư thanh toán BHYT 19 theo đối tượng thẻ Trừ các đối tượng trong báo MRS00216 và MRS00217 (Lấy tất cả
    /// các thẻ bhyt,Trừ các loại thẻ được Cấu hình trong HeinCardNumber__HeinType__01 và HeinCardNumber__HeinType__02); 
    /// </summary>
    public class Mrs00218Filter
    {
        public long BRANCH_ID { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00218Filter() { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00224
{
    /// <summary>
    /// Báo cáo thống kê dịch vụ kỹ thuật thanh toán BHYT 21 theo đối tượng thẻ Trừ các đối tượng trong báo MRS0022 và MRS00223 
    /// (Lấy tất cảcác thẻ bhyt,Trừ các loại thẻ được Cấu hình trong HeinCardNumber__HeinType__01 và 
    /// HeinCardNumber__HeinType__02); 
    /// </summary>
    public class Mrs00224Filter
    {
        public long BRANCH_ID { get; set; }

        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }

        public List<long> BRANCH_IDs { get; set; }

        public Mrs00224Filter() { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00221
{
    /// <summary>
    /// Báo cáo thống kê thuốc thanh toán BHYT 20 theo đối tượng thẻ Trừ các đối tượng trong báo MRS00219 và MRS00220 (Lấy tất cả
    /// các thẻ bhyt,Trừ các loại thẻ được Cấu hình trong HeinCardNumber__HeinType__01 và HeinCardNumber__HeinType__02); 
    /// </summary>
    public class Mrs00221Filter
    {
        public long BRANCH_ID { get; set; }

        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }

        public List<long> BRANCH_IDs { get; set; }

        public Mrs00221Filter() { }
    }
}

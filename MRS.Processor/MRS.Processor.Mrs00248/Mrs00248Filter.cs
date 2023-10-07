using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00248
{
    /// <summary>
    /// Báo cáo tổng hợp đề nghị thanh toán bệnh nhân nội trú bhyt C80a - loại đối tượng 03 (Sử dụng thẻ cấu hình để cấu hình 
    /// loại thẻ, Trừ các đối tượng trong 2 thẻ cấu hình HeinCardNumber__HeinType__01, HeinCardNumber__HeinType__02); 
    /// </summary>
    public class Mrs00248Filter
    {
        public long BRANCH_ID { get; set; }

        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? DEPARTMENT_ID { get; set; }

        public List<long> BRANCH_IDs { get; set; }

        public Mrs00248Filter() { }
    }
}

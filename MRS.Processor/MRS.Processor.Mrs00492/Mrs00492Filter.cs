using HTC.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00492
{
    /// <summary>
    /// Báo cáo File mềm danh sách bệnh nhân nội trú đề nghị thanh toán bhyt C80a - loại đối tượng 03 (Sử dụng thẻ cấu hình để 
    /// cấu hình loại thẻ, Trừ các đối tượng trong 2 thẻ cấu hình HeinCardNumber__HeinType__01, HeinCardNumber__HeinType__02); 
    /// </summary>
    public class Mrs00492Filter : HtcRevenueFilter
    {
        public long? DEPARTMENT_ID { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; }
    }
}

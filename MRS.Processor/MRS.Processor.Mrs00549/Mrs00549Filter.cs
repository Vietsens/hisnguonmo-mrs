using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00549
{
    /// <summary>
    /// Báo cáo đề nghị thánh toán bhyt bệnh nhân kcb ngoại trú C79a theo đối tượng thẻ. Trừ các đối tượng trong báo MRS00210 và 
    /// MRS00211 (Lấy tất cả các thẻ bhyt, Trừ các loại thẻ được Cấu hình trong HeinCardNumber__HeinType__01 và 
    /// HeinCardNumber__HeinType__02); 
    /// </summary>
    public class Mrs00549Filter
    {
        public long? BRANCH_ID { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public List<long> BRANCH_IDs { get; set; }

        public Mrs00549Filter() { }
    }
}

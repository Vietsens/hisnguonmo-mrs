using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00119
{
    /// <summary>
    /// Báo cáo thống kê số lượng hồ sơ điều trị và phân loại theo từng đối tượng bảo hiểm quân đội và các loại khác
    /// </summary>
    public class Mrs00119Filter : FilterBase
    {
        public long? DEPARTMENT_ID { get;  set;  }
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00119Filter()
            : base()
        {

        }
    }
}

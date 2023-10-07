using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00372
{
    /// <summary>
    /// báo cáo bệnh nhân ngoại trú làm dịch vụ theo nhóm
    /// </summary>
    public class Mrs00372Filter : FilterBase
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public long SERVICE_TYPE_ID { get;  set;  }
        public long SERVICE_ID { get;  set;  }
        public long BRANCH_ID { get;  set;  }

        public Mrs00372Filter()
            : base()
        {

        }
    }
}

using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00363
{
    /// <summary>
    /// báo cáo số bệnh nhân kết thúc khám
    /// </summary>
    public class Mrs00363Filter : FilterBase
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public List<long> DEPARTMENT_IDs { get;  set;  }
        public List<long> ROOM_IDs { get;  set;  }

        public Mrs00363Filter()
            : base()
        {

        }
    }
}

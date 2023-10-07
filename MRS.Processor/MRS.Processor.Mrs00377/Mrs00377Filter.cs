using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00377
{
    /// <summary>
    /// báo cáo bệnh nhân ngoại trú làm dịch vụ theo nhóm
    /// </summary>
    public class Mrs00377Filter : FilterBase
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public bool IS_PROGRAM { get;  set;  }
        public Mrs00377Filter()
            : base()
        {

        }
    }
}

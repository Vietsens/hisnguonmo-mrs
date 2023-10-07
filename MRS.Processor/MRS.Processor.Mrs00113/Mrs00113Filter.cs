using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00113
{
    /// <summary>
    /// Báo cáo danh sách bệnh nhân công nợ theo đối tượng bệnh nhân
    /// </summary>
    class Mrs00113Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public long? PATIENT_TYPE_ID { get;  set;  }

        public Mrs00113Filter()
            : base()
        {
        }
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00086
{
    /// <summary>
    /// Báo cáo sổ thống kê chi tiết bệnh nhân điều trị ngoại trú - nội trú
    /// </summary>
    class Mrs00086Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public long TREATMENT_TYPE_ID { get;  set;  }
        //public List<long> TREATMENT_TYPE_IDs { get;  set;  }

        public Mrs00086Filter()
            : base()
        {
        }
    }
}

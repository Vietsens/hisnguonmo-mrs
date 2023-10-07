using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00103
{
    /// <summary>
    /// Báo cáo chi tiết doanh thu bệnh nhân theo khoa
    /// </summary>
    class Mrs00103Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public long? DEPARTMENT_ID { get;  set;  }
        public long? CASHIER_ROOM_ID { get;  set;  }

        public long? TREATMENT_TYPE_ID { get;  set;  }
        public List<long> TREATMENT_TYPE_IDs { get;  set;  }

        public Mrs00103Filter()
            : base()
        {
        }
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00026
{
    /// <summary>
    /// Bao cao tong hop doanh thu theo dich vu
    /// </summary>
    class Mrs00026Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public long? PATIENT_TYPE_ID { get;  set;  }
        public long? TREATMENT_TYPE_ID { get;  set;  }
        public List<long> TREATMENT_TYPE_IDs { get;  set;  }

        public Mrs00026Filter()
            : base()
        {
        }
    }
}

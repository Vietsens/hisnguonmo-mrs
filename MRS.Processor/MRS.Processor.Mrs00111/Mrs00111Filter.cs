using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00111
{
    /// <summary>
    /// Báo cáo sổ ra viên của loại đối tượng bảo hiểm y tế
    /// </summary>
    class Mrs00111Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public long? DEPARTMENT_ID { get;  set;  }

        public string HEIN_NUMBER_CODE { get;  set;  }

        public bool? IS_BHYT_100 { get; set; }
        
        public List<long> TREATMENT_TYPE_IDs { get; set; }

        public Mrs00111Filter()
            : base()
        {
        }
    }
}

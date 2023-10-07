using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00252
{
    public class Mrs00252Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public long? DEPARTMENT_ID { get;  set;  }
        public bool? IS_MEDICINE { get;  set;  }
        public bool? IS_MATERIAL { get; set; }
        public long? MEDI_STOCK_ID { get; set; }
        public long? EXP_MEST_TYPE_ID { get; set; }
    }
}

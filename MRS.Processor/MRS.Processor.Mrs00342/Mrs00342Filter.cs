using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00342
{
    
    public class Mrs00342Filter
    {
        public long TIME_FROM { get;  set;  }                 
        public long TIME_TO { get;  set;  }
        public long? MEDI_STOCK_ID { get;  set;  }
        public long? MEDICINE_TYPE_ID { get;  set;  }
        public long? MATERIAL_TYPE_ID { get;  set;  }

        public Mrs00342Filter() { }
    }
}

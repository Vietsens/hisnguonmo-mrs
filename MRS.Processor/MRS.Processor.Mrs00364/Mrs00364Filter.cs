using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Filter
{
    public class Mrs00364Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public long DEPARTMENT_ID { get;  set;  }
        public List<long> DEPARTMENT_IDs { get; set; }
        public long? MEDI_STOCK_ID { get;  set; }
        
    }
}

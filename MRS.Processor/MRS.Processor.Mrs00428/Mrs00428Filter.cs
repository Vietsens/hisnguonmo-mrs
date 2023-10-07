using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00428
{
    public class Mrs00428Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public long MEDI_STOCK_ID { get;  set; }
        public string KEY_GROUP_EXP { get; set; }
    }
}

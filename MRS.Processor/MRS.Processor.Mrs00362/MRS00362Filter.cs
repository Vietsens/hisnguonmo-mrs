using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.MRS00362
{
    public class Mrs00362Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public List<long> MEDI_BIG_STOCK_IDs { get; set; }
        public List<long> EXP_MEST_REASON_IDs { get; set; }
        public bool? IS_GROUP_TO_PARENT { get; set; }
        public string KEY_GROUP_REASON { get; set; }
    }
}

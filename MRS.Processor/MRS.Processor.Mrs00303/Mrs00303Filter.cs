using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00303
{
    public class Mrs00303Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public bool? IS_ALL_EXP_MEST_TYPE { get; set; }
        public bool? IS_IMP_PRICE { get; set; }
        public List<long> EXP_MEST_TYPE_IDs { get; set; }
    }
}

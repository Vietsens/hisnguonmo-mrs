using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00687
{
    public class Mrs00687Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }
        public long? MEDI_STOCK_ID { get; set; }
        public bool? TRUE_FALSE { get; set; }
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00299
{
    public class Mrs00299Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
    }
}

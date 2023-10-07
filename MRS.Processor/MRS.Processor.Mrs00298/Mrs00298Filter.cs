using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00298
{
    public class Mrs00298Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public long? TIME { get; set; }
        public bool IS_NOT_BILL { get; set; }
        public List<long> BRANCH_IDs { get; set; }
    }
}

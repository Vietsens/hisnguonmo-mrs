using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceReq;
using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00035
{
    public class Mrs00035Filter
    {
        public long? INTRUCTION_TIME_FROM { get; set; }
        public long? INTRUCTION_TIME_TO { get; set; }
        public long? FINISH_TIME_FROM { get; set; }
        public long? FINISH_TIME_TO { get; set; }
        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }
    }
}

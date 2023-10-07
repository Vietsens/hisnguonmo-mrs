using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00189
{
    public class Mrs00189Filter
    {
        public long START_TIME_FROM { get; set; }
        public long START_TIME_TO { get; set; }
        public List<long> SERVICE_REQ_TYPE_IDs { get; set; }
    }
}

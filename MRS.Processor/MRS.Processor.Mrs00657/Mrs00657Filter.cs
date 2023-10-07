using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00657
{
    public class Mrs00657Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }

        public long? OUT_TIME_FROM { get; set; }
        public long? OUT_TIME_TO { get; set; }

        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00651
{
    public class Mrs00651Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }

        public List<string> CASHIER_LOGINNAMEs { get; set; }
    }
}

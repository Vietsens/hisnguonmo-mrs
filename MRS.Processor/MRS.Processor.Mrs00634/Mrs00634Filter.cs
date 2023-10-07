using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00634
{
    public class Mrs00634Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }

        public bool? IS_BHYT { get; set; }
        public string CASHIER_LOGINNAME { get; set; }
        public List<string> CASHIER_LOGINNAMEs { get; set; }

        public long? EXACT_CASHIER_ROOM_ID { get; set; }
    }
}

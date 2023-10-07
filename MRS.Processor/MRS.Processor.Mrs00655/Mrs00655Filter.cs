using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00655
{
    public class Mrs00655Filter
    {
        public string CASHIER_LOGINNAME { get; set; }
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public long? EXACT_CASHIER_ROOM_ID { get; set; }
    }
}

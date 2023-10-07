using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00624
{
    public class Mrs00624Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public string CANCEL_LOGINNAME { get; set; }
        public long? EXACT_CASHIER_ROOM_ID { get; set; }

        public List<long> TRANSACTION_TYPE_IDs { get; set; }
    }
}

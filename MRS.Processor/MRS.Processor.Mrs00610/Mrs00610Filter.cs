using MOS.MANAGER.HisServiceReq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00610
{
    public class Mrs00610Filter
    {
        public long? FEE_LOCK_TIME_FROM { get; set; }
        public long? FEE_LOCK_TIME_TO { get; set; }
        public long? TRANSACTION_TIME_FROM { get; set; }
        public long? TRANSACTION_TIME_TO { get; set; }
        public long? REPORT_TYPE_CAT_ID { get; set; }
        public List<long> REPORT_TYPE_CAT_IDs { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace MOS.Filter
{
    public class HisCareFilter : FilterBase
    {
        public long? TREATMENT_ID { get; set; }
        public long? AWARENESS_ID { get; set; }
        public long? EXECUTE_TIME_FROM { get; set; }
        public long? EXECUTE_TIME_TO { get; set; }
        public long? CARE_SUM_ID { get; set; }
        public long? DHST_ID { get; set; }
        public long? TRACKING_ID { get; set; }

        public HisCareFilter()
            : base()
        {
        }
    }
}

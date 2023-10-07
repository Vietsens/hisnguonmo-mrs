
using System.Collections.Generic;
namespace HTC.Filter
{
    public class HtcRevenueFilter : FilterBase
    {
        public long? PERIOD_ID { get; set; }

        public List<long> PERIOD_IDs { get; set; }

        public long? REVENUE_TIME_FROM { get; set; }
        public long? REVENUE_TIME_TO { get; set; }

        public long? IN_TIME_FROM { get; set; }
        public long? IN_TIME_TO { get; set; }

        public long? OUT_TIME_FROM { get; set; }
        public long? OUT_TIME_TO { get; set; }

        public string REQUEST_DEPARTMENT_CODE_EXACT { get; set; }
        public string EXECUTE_DEPARTMENT_CODE_EXACT { get; set; }

        public HtcRevenueFilter()
            : base()
        {
        }
    }
}

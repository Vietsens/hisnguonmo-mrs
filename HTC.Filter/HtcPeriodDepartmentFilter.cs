
using System.Collections.Generic;
namespace HTC.Filter
{
    public class HtcPeriodDepartmentFilter : FilterBase
    {
        public long? PERIOD_ID { get; set; }

        public List<long> PERIOD_IDs { get; set; }

        public HtcPeriodDepartmentFilter()
            : base()
        {
        }
    }
}


using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisTrackingFilter : FilterBase
    {

        public List<long> TREATMENT_IDs { get; set; }
        public long? TREATMENT_ID { get; set; }
        public long? TRACKING_TIME_FROM { get; set; }
        public long? TRACKING_TIME_TO { get; set; }

        public HisTrackingFilter()
            : base()
        {
        }
    }
}

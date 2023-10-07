
namespace MOS.Filter
{
    public class HisTrackingViewFilter : FilterBase
    {
        public long? TREATMENT_ID { get; set; }
        public long? TRACKING_TIME_FROM { get; set; }
        public long? TRACKING_TIME_TO { get; set; }

        public HisTrackingViewFilter()
            : base()
        {
        }
    }
}


namespace MOS.Filter
{
    public class HisVitaminAViewFilter : FilterBase
    {
        public long? REQUEST_TIME_FROM { get; set; }
        public long? REQUEST_TIME_TO { get; set; }

        public long? EXECUTE_TIME_FROM { get; set; }
        public long? EXECUTE_TIME_TO { get; set; }

        public HisVitaminAViewFilter()
            : base()
        {
        }
    }
}

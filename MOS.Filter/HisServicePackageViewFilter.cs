
namespace MOS.Filter
{
    public class HisServicePackageViewFilter : FilterBase
    {
        public long? SERVICE_ID { get; set; }
        public long? SERVICE_ATTACH_ID { get; set; }

        public HisServicePackageViewFilter()
            : base()
        {

        }
    }
}

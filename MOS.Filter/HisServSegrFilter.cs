
namespace MOS.Filter
{
    public class HisServSegrFilter : FilterBase
    {
        public long? SERVICE_ID { get; set; }
        public long? SERVICE_GROUP_ID { get; set; }

        public HisServSegrFilter()
            : base()
        {
        }
    }
}


namespace MOS.Filter
{
    public class HisServiceMetyFilter : FilterBase
    {
        public long? SERVICE_ID { get; set; }
        public long? MEDICINE_TYPE_ID { get; set; }

        public HisServiceMetyFilter()
            : base()
        {
        }
    }
}

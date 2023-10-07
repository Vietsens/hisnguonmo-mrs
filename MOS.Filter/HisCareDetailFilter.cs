
namespace MOS.Filter
{
    public class HisCareDetailFilter : FilterBase
    {
        public long? CARE_ID { get; set; }
        public long? CARE_TYPE_ID { get; set; }

        public HisCareDetailFilter()
            : base()
        {
        }
    }
}

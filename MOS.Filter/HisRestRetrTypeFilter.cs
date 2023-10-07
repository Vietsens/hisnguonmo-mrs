
namespace MOS.Filter
{
    public class HisRestRetrTypeFilter : FilterBase
    {
        public long? REHA_TRAIN_TYPE_ID { get; set; }
        public long? REHA_SERVICE_TYPE_ID { get; set; }

        public HisRestRetrTypeFilter()
            : base()
        {
        }
    }
}

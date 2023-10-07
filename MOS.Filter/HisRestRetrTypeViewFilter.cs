
namespace MOS.Filter
{
    public class HisRestRetrTypeViewFilter : FilterBase
    {
        public long? REHA_TRAIN_UNIT_ID { get; set; }
        public long? SERVICE_ID { get; set; }
        public long? REHA_TRAIN_TYPE_ID { get; set; }
        public long? REHA_SERVICE_TYPE_ID { get; set; }

        public HisRestRetrTypeViewFilter()
            : base()
        {
        }
    }
}

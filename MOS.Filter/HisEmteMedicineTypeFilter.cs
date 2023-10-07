
namespace MOS.Filter
{
    public class HisEmteMedicineTypeFilter : FilterBase
    {
        public long? EXP_MEST_TEMPLATE_ID { get; set; }
        public long? MEDICINE_TYPE_ID { get; set; }
        public HisEmteMedicineTypeFilter()
            : base()
        {
        }
    }
}

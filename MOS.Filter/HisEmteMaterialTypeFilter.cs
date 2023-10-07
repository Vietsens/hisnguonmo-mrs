
namespace MOS.Filter
{
    public class HisEmteMaterialTypeFilter : FilterBase
    {
        public long? EXP_MEST_TEMPLATE_ID { get; set; }
        public long? MATERIAL_TYPE_ID { get; set; }

        public HisEmteMaterialTypeFilter()
            : base()
        {
        }
    }
}

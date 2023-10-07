
namespace MOS.Filter
{
    public class HisAcinInteractiveFilter : FilterBase
    {
        public long? ACTIVE_INGREDIENT_ID { get; set; }
        public long? ACTIVE_INGREDIENT_CONFLICT_ID { get; set; }
        public long? ACTIVE_INGREDIENT_ID__OR__ACTIVE_INGREDIENT_CONFLICT_ID { get; set; }

        public HisAcinInteractiveFilter()
            : base()
        {
        }
    }
}

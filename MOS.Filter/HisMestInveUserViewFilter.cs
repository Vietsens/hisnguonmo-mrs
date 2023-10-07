
namespace MOS.Filter
{
    public class HisMestInveUserViewFilter : FilterBase
    {
        public long? MEST_INVENTORY_ID { get; set; }
        public long? EXECUTE_ROLE_ID { get; set; }
        
        public HisMestInveUserViewFilter()
            : base()
        {
        }
    }
}

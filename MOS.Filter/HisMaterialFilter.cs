
namespace MOS.Filter
{
    public class HisMaterialFilter : FilterBase
    {
        public long? MATERIAL_TYPE_ID { get; set; }
        public long? SUPPLIER_ID { get; set; }
        public long? BID_ID { get; set; }

        public HisMaterialFilter()
            : base()
        {
        }
    }
}

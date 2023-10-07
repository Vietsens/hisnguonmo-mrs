
namespace MOS.Filter
{
    public class HisMediStockMatyFilter : FilterBase
    {
        public long? MATERIAL_TYPE_ID { get; set; }
        public long? MEDI_STOCK_ID { get; set; }

        public HisMediStockMatyFilter()
            : base()
        {
        }
    }
}

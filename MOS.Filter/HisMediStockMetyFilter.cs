
namespace MOS.Filter
{
    public class HisMediStockMetyFilter : FilterBase
    {
        public long? MEDI_STOCK_ID { get; set; }
        public long? MEDICINE_TYPE_ID { get; set; }

        public HisMediStockMetyFilter()
            : base()
        {
        }
    }
}

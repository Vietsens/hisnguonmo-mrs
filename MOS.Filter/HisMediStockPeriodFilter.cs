
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisMediStockPeriodFilter : FilterBase
    {
        public long? MEDI_STOCK_ID { get; set; }
        public long? PREVIOUS_ID { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public long? TO_TIME_FROM { get; set; }
        public long? TO_TIME_TO { get; set; }

        public HisMediStockPeriodFilter()
            : base()
        {
        }
    }
}


using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisMediStockExtyFilter : FilterBase
    {
        public long? MEDI_STOCK_ID { get; set; }
        public long? EXP_MEST_TYPE_ID { get; set; }

        public List<long> MEDI_STOCK_IDs { get; set; }
        public List<long> EXP_MEST_TYPE_IDs { get; set; }

        public HisMediStockExtyFilter()
            : base()
        {
        }
    }
}

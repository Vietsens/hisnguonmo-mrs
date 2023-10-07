
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisMediStockImtyFilter : FilterBase
    {

        public long? MEDI_STOCK_ID { get; set; }
        public long? IMP_MEST_TYPE_ID { get; set; }

        public List<long> MEDI_STOCK_IDs { get; set; }
        public List<long> IMP_MEST_TYPE_IDs { get; set; }

        public HisMediStockImtyFilter()
            : base()
        {
        }
    }
}

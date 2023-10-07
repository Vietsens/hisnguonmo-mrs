
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisMediStockMatyViewFilter : FilterBase
    {
        public long? MEDI_STOCK_ID { get; set; }
        public List<long> MATERIAL_TYPE_IDs { get; set; }

        public HisMediStockMatyViewFilter()
            : base()
        {
        }
    }
}

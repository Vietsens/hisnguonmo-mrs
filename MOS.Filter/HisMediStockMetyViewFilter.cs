
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisMediStockMetyViewFilter : FilterBase
    {
        public long? MEDI_STOCK_ID { get; set; }
        public List<long> MEDICINE_TYPE_IDs { get; set; }

        public HisMediStockMetyViewFilter()
            : base()
        {
        }
    }
}

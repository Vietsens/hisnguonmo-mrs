
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisMestPeriodMatyFilter : FilterBase
    {
        public long? MEDI_STOCK_PERIOD_ID { get; set; }
        public long? MATERIAL_TYPE_ID { get; set; }

        public List<long> MEDI_STOCK_PERIOD_IDs { get; set; }
        public List<long> MATERIAL_TYPE_IDs { get; set; }

        public HisMestPeriodMatyFilter()
            : base()
        {
        }
    }
}

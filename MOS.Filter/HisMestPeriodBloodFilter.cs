
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisMestPeriodBloodFilter : FilterBase
    {
        public List<long> BLOOD_IDs { get; set; }
        public long? BLOOD_ID { get; set; }
        public long? MEDI_STOCK_PERIOD_ID { get; set; }
        public List<long> MEDI_STOCK_PERIOD_IDs { get; set; }

        public HisMestPeriodBloodFilter()
            : base()
        {
        }
    }
}

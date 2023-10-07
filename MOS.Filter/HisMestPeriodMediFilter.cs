
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisMestPeriodMediFilter : FilterBase
    {
        public long? MEDI_STOCK_PERIOD_ID { get; set; }
        public long? MEDICINE_ID { get; set; }

        public List<long> MEDI_STOCK_PERIOD_IDs { get; set; }
        public List<long> MEDICINE_IDs { get; set; }
        
        public HisMestPeriodMediFilter()
            : base()
        {
        }
    }
}

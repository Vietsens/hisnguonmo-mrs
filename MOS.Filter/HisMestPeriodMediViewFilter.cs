
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisMestPeriodMediViewFilter : FilterBase
    {
        public long? MEDI_STOCK_PERIOD_ID { get; set; }
        public long? MEDICINE_ID { get; set; }
        public long? MEDICINE_TYPE_ID { get; set; }
        public long? BID_ID { get; set; }
        public long? SUPPLIER_ID { get; set; }
        public long? SERVICE_ID { get; set; }

        public List<long> MEDI_STOCK_PERIOD_IDs { get; set; }
        public List<long> MEDICINE_IDs { get; set; }
        public List<long> MEDICINE_TYPE_IDs { get; set; }
        public List<long> BID_IDs { get; set; }
        public List<long> SUPPLIER_IDs { get; set; }
        public List<long> SERVICE_IDs { get; set; }

        public HisMestPeriodMediViewFilter()
            : base()
        {
        }
    }
}

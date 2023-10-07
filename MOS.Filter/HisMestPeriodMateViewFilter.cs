
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisMestPeriodMateViewFilter : FilterBase
    {
        public long? MEDI_STOCK_PERIOD_ID { get; set; }
        public long? MATERIAL_ID { get; set; }
        public long? MATERIAL_TYPE_ID { get; set; }
        public long? BID_ID { get; set; }
        public long? SUPPLIER_ID { get; set; }
        public long? SERVICE_ID { get; set; }

        public List<long> MEDI_STOCK_PERIOD_IDs { get; set; }
        public List<long> MATERIAL_IDs { get; set; }
        public List<long> MATERIAL_TYPE_IDs { get; set; }
        public List<long> BID_IDs { get; set; }
        public List<long> SUPPLIER_IDs { get; set; }
        public List<long> SERVICE_IDs { get; set; }

        public HisMestPeriodMateViewFilter()
            : base()
        {
        }
    }
}

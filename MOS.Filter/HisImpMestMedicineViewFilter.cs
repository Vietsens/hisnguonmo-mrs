using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisImpMestMedicineViewFilter : FilterBase
    {
        public List<long> IMP_MEST_IDs { get; set; }
        public List<long> IMP_MEST_STT_IDs { get; set; }
        public List<long> MEDICINE_IDs { get; set; }
        public long? IMP_MEST_ID { get; set; }
        public long? MEDICINE_ID { get; set; }
        public long? MEDI_STOCK_PERIOD_ID { get; set; }
        public bool? HAS_MEDI_STOCK_PERIOD { get; set; }
        public long? MEDICINE_TYPE_ID { get; set; }
        public long? IMP_MEST_STT_ID { get; set; }
        public long? MEDI_STOCK_ID { get; set; }
        public long? IMP_TIME_FROM { get; set; }
        public long? IMP_TIME_TO { get; set; }
        public long? IMP_MEST_TYPE_ID { get; set; }
        public long? AGGR_IMP_MEST_ID { get; set; }
        public List<long> MEDICINE_TYPE_IDs { get; set; }
        public List<long> SERVICE_IDs { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }

        public HisImpMestMedicineViewFilter()
            : base()
        {
        }
    }
}

using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisImpMestMedicineFilter : FilterBase
    {
        public List<long> MEDICINE_IDs { get; set; }
        public List<long> IMP_MEST_IDs { get; set; }

        public long? IMP_MEST_ID { get; set; }
        public long? MEDICINE_ID { get; set; }

        public long? IMP_MEST_ID__NOT__EQUAL { get; set; }

        public HisImpMestMedicineFilter()
            : base()
        {
        }
    }
}

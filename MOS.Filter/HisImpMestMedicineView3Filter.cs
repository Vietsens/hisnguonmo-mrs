using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisImpMestMedicineView3Filter : FilterBase
    {
        public List<long> IMP_MEST_IDs { get; set; }
        public List<long> MEDICINE_IDs { get; set; }
        public long? IMP_MEST_ID { get; set; }
        public long? MEDICINE_ID { get; set; }
        public long? MEDICINE_TYPE_ID { get; set; }
        public long? AGGR_IMP_MEST_ID { get; set; }
        public List<long> MEDICINE_TYPE_IDs { get; set; }
        public long? IMP_TIME_FROM { get; set; }
        public long? IMP_TIME_TO { get; set; }
        
        public HisImpMestMedicineView3Filter()
            : base()
        {
        }
    }
}

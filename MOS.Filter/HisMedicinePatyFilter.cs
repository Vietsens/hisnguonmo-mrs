using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisMedicinePatyFilter : FilterBase
    {
        public long? PATIENT_TYPE_ID { get; set; }
        public long? MEDICINE_ID { get; set; }
        public List<long> MEDICINE_IDs { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public long? ID__NOT_EQUAL { get; set; }

        public HisMedicinePatyFilter()
            : base()
        {
        }
    }
}

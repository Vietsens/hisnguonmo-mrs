using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisMaterialPatyFilter : FilterBase
    {
        public long? PATIENT_TYPE_ID { get; set; }
        public long? MATERIAL_ID { get; set; }
        public List<long> MATERIAL_IDs { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public long? ID__NOT_EQUAL { get; set; }

        public HisMaterialPatyFilter()
            : base()
        {
        }
    }
}

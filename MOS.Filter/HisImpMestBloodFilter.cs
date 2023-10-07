
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisImpMestBloodFilter : FilterBase
    {
        public long? IMP_MEST_ID { get; set; }
        public long? BLOOD_ID { get; set; }
        public List<long> IMP_MEST_IDs { get; set; }
        public List<long> BLOOD_IDs { get; set; }

        public long? IMP_MEST_ID__NOT__EQUAL { get; set; }

        public HisImpMestBloodFilter()
            : base()
        {
        }
    }
}

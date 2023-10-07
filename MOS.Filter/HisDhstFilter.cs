
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisDhstFilter : FilterBase
    {
        public long? TREATMENT_ID { get; set; }
        public long? TRACKING_ID { get; set; }
        public long? CARE_ID { get; set; }
        public List<long> CARE_IDs { get; set; }
        public List<long> TRACKING_IDs { get; set; }
        public List<long> TREATMENT_IDs { get; set; }

        public HisDhstFilter()
            : base()
        {
        }
    }
}

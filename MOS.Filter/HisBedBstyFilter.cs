
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisBedBstyFilter : FilterBase
    {
        public long? BED_ID { get; set; }
        public long? BED_SERVICE_TYPE_ID { get; set; }

        public List<long> BED_IDs { get; set; }
        public List<long> BED_SERVICE_TYPE_IDs { get; set; }

        public HisBedBstyFilter()
            : base()
        {
        }
    }
}


using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisBidBloodTypeViewFilter : FilterBase
    {
        public long? BID_ID { get; set; }
        public long? BLOOD_TYPE_ID { get; set; }
        public List<long> BID_IDs { get; set; }
        public List<long> BLOOD_TYPE_IDs { get; set; }

        public HisBidBloodTypeViewFilter()
            : base()
        {
        }
    }
}

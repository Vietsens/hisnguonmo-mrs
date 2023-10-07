
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisAnticipateBltyFilter : FilterBase
    {
        public List<long> ANTICIPATE_IDs { get; set; }
        public List<long> BLOOD_TYPE_IDs { get; set; }
        public long? ANTICIPATE_ID { get; set; }
        public long? BLOOD_TYPE_ID { get; set; }

        public HisAnticipateBltyFilter()
            : base()
        {
        }
    }
}

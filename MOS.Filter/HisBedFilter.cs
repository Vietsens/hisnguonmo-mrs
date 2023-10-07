
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisBedFilter : FilterBase
    {
        public long? BED_TYPE_ID { get; set; }
        public List<long> BED_TYPE_IDs { get; set; }

        public HisBedFilter()
            : base()
        {
        }
    }
}

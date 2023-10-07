
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisBloodTypeFilter : FilterBase
    {
        public long? PARENT_ID { get; set; }
        public List<long> PARENT_IDs { get; set; }

        public HisBloodTypeFilter()
            : base()
        {
        }
    }
}

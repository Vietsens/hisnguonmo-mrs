
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisMestInveUserFilter : FilterBase
    {
        public long? MEST_INVENTORY_ID { get; set; }
        public List<long> MEST_INVENTORY_IDs { get; set; }

        public HisMestInveUserFilter()
            : base()
        {
        }
    }
}

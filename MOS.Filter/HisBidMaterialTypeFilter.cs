
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisBidMaterialTypeFilter : FilterBase
    {
        public List<long> MATERIAL_TYPE_IDs { get; set; }
        public List<long> BID_IDs { get; set; }
        public long? MATERIAL_TYPE_ID { get; set; }
        public long? BID_ID { get; set; }

        public HisBidMaterialTypeFilter()
            : base()
        {
        }
    }
}

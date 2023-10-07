
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisAnticipateMatyViewFilter : FilterBase
    {
        public long? ANTICIPATE_ID { get; set; }
        public long? MATERIAL_TYPE_ID { get; set; }
        public List<long> MATERIAL_TYPE_IDs { get; set; }
        public List<long> ANTICIPATE_IDs { get; set; }

        public HisAnticipateMatyViewFilter()
            : base()
        {
        }
    }
}


using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisServiceMetyViewFilter : FilterBase
    {
        public long? SERVICE_ID { get; set; }
        public long? MEDICINE_TYPE_ID { get; set; }
        public long? SERVICE_TYPE_ID { get; set; }
        public List<long> SERVICE_IDs { get; set; }
        public List<long> SERVICE_TYPE_IDs { get; set; }

        public HisServiceMetyViewFilter()
            : base()
        {
        }
    }
}

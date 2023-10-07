using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisServiceViewFilter : FilterBase
    {
        public long? SERVICE_TYPE_ID { get; set; }
        public bool? IS_LEAF { get; set; }
        public List<string> SERVICE_CODEs { get; set; }

        public HisServiceViewFilter()
            : base()
        {
        }
    }
}

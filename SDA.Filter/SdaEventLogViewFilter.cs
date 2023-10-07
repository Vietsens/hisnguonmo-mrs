
using System.Collections.Generic;
namespace SDA.Filter
{
    public class SdaEventLogViewFilter : FilterBase
    {
        public List<long> EVENT_LOG_TYPE_IDs { get; set; }
        public string LOGIN_NAME { get; set; }

        public SdaEventLogViewFilter()
            : base()
        {
        }
    }
}

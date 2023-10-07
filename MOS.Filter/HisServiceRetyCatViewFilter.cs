
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisServiceRetyCatViewFilter : FilterBase
    {
        public List<long> REPORT_TYPE_CAT_IDs { get; set; }
        public long? REPORT_TYPE_CAT_ID { get; set; }
        public long? SERVICE_ID { get; set; }
        public string REPORT_TYPE_CODE__EXACT { get; set; }
        public List<long> SERVICE_IDs { get; set; }

        public HisServiceRetyCatViewFilter()
            : base()
        {
        }
    }
}

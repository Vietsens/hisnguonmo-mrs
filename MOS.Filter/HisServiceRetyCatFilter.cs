
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisServiceRetyCatFilter : FilterBase
    {
        public List<long> REPORT_TYPE_CAT_IDs { get; set; }
        public long? REPORT_TYPE_CAT_ID { get; set; }
        public long? SERVICE_ID { get; set; }
        public List<long> SERVICE_IDs { get; set; }
        
        public HisServiceRetyCatFilter()
            : base()
        {
        }
    }
}

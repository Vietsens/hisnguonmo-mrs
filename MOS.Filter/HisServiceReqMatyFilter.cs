
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisServiceReqMatyFilter : FilterBase
    {
        public long? SERVICE_REQ_ID { get; set; }
        public long? MATERIAL_TYPE_ID { get; set; }
        public List<long> SERVICE_REQ_IDs { get; set; }
        public List<long> MATERIAL_TYPE_IDs { get; set; }

        public HisServiceReqMatyFilter()
            : base()
        {
        }
    }
}

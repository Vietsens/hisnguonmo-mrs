
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisServiceReqMetyFilter : FilterBase
    {
        public long? SERVICE_REQ_ID { get; set; }
        public long? MEDICINE_TYPE_ID { get; set; }
        public List<long> SERVICE_REQ_IDs { get; set; }
        public List<long> MEDICINE_TYPE_IDs { get; set; }

        public HisServiceReqMetyFilter()
            : base()
        {
        }
    }
}


using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisServiceRoomViewFilter : FilterBase
    {
        public List<long> SERVICE_IDS { get; set; }
        public List<long> SERVICE_TYPE_IDs { get; set; }
        public List<long> BRANCH_IDs { get; set; }
        public long? SERVICE_ID { get; set; }
        public long? ROOM_TYPE_ID { get; set; }
        public long? SERVICE_TYPE_ID { get; set; }
        public long? ROOM_ID { get; set; }
        public long? PARENT_ID { get; set; }
        public bool? IS_LEAF { get; set; }
        public long? BRANCH_ID { get; set; }

        public HisServiceRoomViewFilter()
            : base()
        {
        }
    }
}

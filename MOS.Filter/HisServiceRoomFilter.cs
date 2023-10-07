using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisServiceRoomFilter : FilterBase
    {
        public long? SERVICE_ID { get; set; }
        public long? ROOM_ID { get; set; }
        public long? ID__NOT_EQUAL { get; set; }
        public List<long> SERVICE_IDS { get; set; }

        public HisServiceRoomFilter()
            : base()
        {
        }
    }
}

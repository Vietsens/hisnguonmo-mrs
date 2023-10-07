
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisExecuteRoomFilter : FilterBase
    {
        public List<long> ROOM_IDs { get; set; }
        public long? ROOM_ID { get; set; }
        public bool? IS_EMERGENCY { get; set; }
        public bool? IS_EXAM { get; set; }

        public HisExecuteRoomFilter()
            : base()
        {
        }
    }
}

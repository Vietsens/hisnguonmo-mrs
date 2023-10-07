
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisBedRoomFilter : FilterBase
    {
        public List<long> ROOM_IDs { get; set; }
        public long? ROOM_ID { get; set; }

        public HisBedRoomFilter()
            : base()
        {
        }
    }
}

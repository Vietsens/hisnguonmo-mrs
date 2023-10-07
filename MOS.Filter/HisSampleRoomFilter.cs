
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisSampleRoomFilter : FilterBase
    {
        public long? ROOM_ID { get; set; }
        public List<long> ROOM_IDs { get; set; }

        public HisSampleRoomFilter()
            : base()
        {
        }
    }
}

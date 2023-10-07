
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisCashierRoomFilter : FilterBase
    {
        public long? ROOM_ID { get; set; }
        public List<long> ROOM_IDs { get; set; }

        public HisCashierRoomFilter()
            : base()
        {
        }
    }
}

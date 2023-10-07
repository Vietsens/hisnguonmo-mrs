
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisUserRoomViewFilter : FilterBase
    {
        public string LOGINNAME { get; set; }
        public long? ROOM_ID { get; set; }
        public List<long> ROOM_IDs { get; set; }

        public HisUserRoomViewFilter()
            : base()
        {
        }
    }
}

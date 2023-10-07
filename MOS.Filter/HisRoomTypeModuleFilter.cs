
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisRoomTypeModuleFilter : FilterBase
    {
        public long? ROOM_TYPE_ID { get; set; }
        public List<long> ROOM_TYPE_IDs { get; set; }
        public string MODULE_LINK__EXACT { get; set; }
        public List<string> MODULE_LINK__EXACTs { get; set; }

        public HisRoomTypeModuleFilter()
            : base()
        {
        }
    }
}

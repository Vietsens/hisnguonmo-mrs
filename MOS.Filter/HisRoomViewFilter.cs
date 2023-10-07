
namespace MOS.Filter
{
    public class HisRoomViewFilter : FilterBase
    {
        public long? DEPARTMENT_ID { get; set; }
        public long? ROOM_TYPE_ID { get; set; }

        public HisRoomViewFilter()
            : base()
        {

        }
    }
}

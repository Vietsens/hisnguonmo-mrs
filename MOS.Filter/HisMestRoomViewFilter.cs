
namespace MOS.Filter
{
    public class HisMestRoomViewFilter : FilterBase
    {
        public long? MEDI_STOCK_ID { get; set; }
        public long? ROOM_ID { get; set; }

        public HisMestRoomViewFilter()
            : base()
        {
        }
    }
}

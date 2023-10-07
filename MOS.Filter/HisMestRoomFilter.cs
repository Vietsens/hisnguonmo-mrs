
namespace MOS.Filter
{
    public class HisMestRoomFilter : FilterBase
    {
        public long? MEDI_STOCK_ID { get; set; }
        public long? ROOM_ID { get; set; }

        public HisMestRoomFilter()
            : base()
        {
        }
    }
}

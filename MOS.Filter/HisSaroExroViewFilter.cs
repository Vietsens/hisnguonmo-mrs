
namespace MOS.Filter
{
    public class HisSaroExroViewFilter : FilterBase
    {
        public long? SAMPLE_ROOM_ID { get; set; }
        public long? EXECUTE_ROOM_ID { get; set; }

        public HisSaroExroViewFilter()
            : base()
        {
        }
    }
}

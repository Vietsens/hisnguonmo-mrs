
namespace SDA.Filter
{
    public class SdaMessageBroadcastViewFilter : FilterBase
    {
        public enum IsRead
        {
            READ,
            UNREAD,
            ALL,
        }
        public IsRead? IS_READ { get; set; }

        public SdaMessageBroadcastViewFilter()
            : base()
        {
        }
    }
}

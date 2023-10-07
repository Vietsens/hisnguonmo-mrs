
namespace TYT.Filter
{
    public class TytGdskFilter : FilterBase
    {
        public TytGdskFilter()
            : base()
        {
        }

        public long? GDSK_TIME_FROM { get; set; }
        public long? GDSK_TIME_TO { get; set; }
    }
}

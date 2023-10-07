
namespace SDA.Filter
{
    public class SdaGroupViewFilter : FilterBase
    {
        public bool? IsTree { get; set; }
        public long? Node { get; set; }

        public SdaGroupViewFilter()
            : base()
        {
        }
    }
}


namespace SDA.Filter
{
    public class SdaFeedbackFilter : FilterBase
    {
        public string PUK { get; set; }
        public string LOGINNAME { get; set; }

        public SdaFeedbackFilter()
            : base()
        {
        }
    }
}

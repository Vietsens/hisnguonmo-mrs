
namespace SDA.Filter
{
    public class SdaLicenseFilter : FilterBase
    {
        public string PUK { get; set; }
        public string APP_CODE { get; set; }
        public string SN_HDD { get; set; }
        public long Time { get; set; }
        public string JsonMessage { get; set; }

        public SdaLicenseFilter()
            : base()
        {
        }
    }
}

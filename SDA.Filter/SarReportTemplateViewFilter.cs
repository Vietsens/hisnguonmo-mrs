
namespace SDA.Filter
{
    public class SarReportTemplateViewFilter : FilterBase
    {
        public string REPORT_TEMPLATE_CODE { get; set; }
        public long? REPORT_TYPE_ID { get; set; }

        public SarReportTemplateViewFilter()
            : base()
        {
        }
    }
}

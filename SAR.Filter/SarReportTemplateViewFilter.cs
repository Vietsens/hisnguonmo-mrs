
namespace SAR.Filter
{
    public class SarReportTemplateViewFilter : FilterBase
    {
        public string REPORT_TEMPLATE_CODE { get; set; }
        public long? REPORT_TYPE_ID { get; set; }
        public string REPORT_TYPE_CODE { get; set; }

        public SarReportTemplateViewFilter()
            : base()
        {
        }
    }
}

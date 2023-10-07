
namespace SAR.Filter
{
    public class SarReportTemplateFilter : FilterBase
    {
        public string REPORT_TEMPLATE_CODE { get; set; }
        public long? REPORT_TYPE_ID { get; set; }

        public SarReportTemplateFilter()
            : base()
        {
        }
    }
}

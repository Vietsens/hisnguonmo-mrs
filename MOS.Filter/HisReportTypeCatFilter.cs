
namespace MOS.Filter
{
    public class HisReportTypeCatFilter : FilterBase
    {
        public string REPORT_TYPE_CODE__EXACT { get; set; }
        public string CATEGORY_CODE__EXACT { get; set; }
        public string CATEGORY_NAME { get; set; }

        public HisReportTypeCatFilter()
            : base()
        {
        }
    }
}

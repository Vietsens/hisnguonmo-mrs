using System.Collections.Generic;

namespace SAR.Filter
{
    public class SarReportFilter : FilterBase
    {
        public long? REPORT_STT_ID { get; set; }
        public List<long> REPORT_STT_IDs { get; set; }

        public enum FilterModeEnum
        {
            ALL,
            CREATE,
            RECEIVE,
        }
        public FilterModeEnum FilterMode { get; set; }

        public SarReportFilter()
            : base()
        {
        }
    }
}

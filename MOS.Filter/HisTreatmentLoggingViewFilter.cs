
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisTreatmentLoggingViewFilter : FilterBase
    {
        public long? TREATMENT_ID { get; set; }
        public List<long> TREATMENT_IDs { get; set; }
        public long? TREATMENT_LOG_TYPE_ID { get; set; }

        public HisTreatmentLoggingViewFilter()
            : base()
        {
        }
    }
}

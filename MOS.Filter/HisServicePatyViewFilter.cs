using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisServicePatyViewFilter : FilterBase
    {
        public long? TREATMENT_TIME { get; set; }
        public long? SERVICE_ID { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public long? SERVICE_TYPE_ID { get; set; }
        public long? BRANCH_ID { get; set; }
        public bool? IN_ACTIVE_TIME { get; set; }

        public List<long> SERVICE_IDs { get; set; }

        public HisServicePatyViewFilter()
            : base()
        {

        }
    }
}

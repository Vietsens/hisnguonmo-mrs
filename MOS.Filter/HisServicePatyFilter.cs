using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisServicePatyFilter : FilterBase
    {
        public List<long> SERVICE_IDs { get; set; }
        public List<long> BRANCH_IDs { get; set; }
        public bool? IN_ACTIVE_TIME { get; set; }
        public long? TREATMENT_TIME { get; set; }
        public long? INSTRUCTION_TIME { get; set; }
        public long? SERVICE_ID { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public long? BRANCH_ID { get; set; }

        public HisServicePatyFilter()
            : base()
        {
        }
    }
}


using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisPrescriptionFilter : FilterBase
    {
        public long? EXP_MEST_ID { get; set; }
        public long? SERVICE_REQ_ID { get; set; }
        public long? TDL_TREATMENT_ID { get; set; }

        public List<long> SERVICE_REQ_IDs { get; set; }
        public List<long> TDL_TREATMENT_IDs { get; set; }

        public HisPrescriptionFilter()
            : base()
        {
        }
    }
}

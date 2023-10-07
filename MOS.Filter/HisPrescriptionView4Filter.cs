
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisPrescriptionView4Filter : FilterBase
    {
        public List<long> EXP_MEST_IDs { get; set; }
        public List<long> SERVICE_REQ_IDs { get; set; }

        public long? EXP_MEST_ID { get; set; }
        public long? SERVICE_REQ_ID { get; set; }
        public long? TREATMENT_ID { get; set; }

        public long? USE_TIME__TO { get; set; }
        public long? USE_TIME__FROM { get; set; }

        public HisPrescriptionView4Filter()
            : base()
        {
        }
    }
}

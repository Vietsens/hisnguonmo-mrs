
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisBabyViewFilter : FilterBase
    {
        public List<long> TREATMENT_IDs { get; set; }
        public List<long> BORN_POSITION_IDs { get; set; }
        public List<long> BORN_TYPE_IDs { get; set; }
        public List<long> BORN_RESULT_IDs { get; set; }

        public long? TREATMENT_ID { get; set; }
        public long? BORN_POSITION_ID { get; set; }
        public long? BORN_TYPE_ID { get; set; }
        public long? BORN_RESULT_ID { get; set; }

        public HisBabyViewFilter()
            : base()
        {
        }
    }
}

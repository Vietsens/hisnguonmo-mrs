
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisAccidentHurtViewFilter : FilterBase
    {
        public List<long> TREATMENT_IDs { get; set; }
        public long? TREATMENT_ID { get; set; }
        public long? ACCIDENT_HURT_TYPE_ID { get; set; }

        public HisAccidentHurtViewFilter()
            : base()
        {
        }
    }
}
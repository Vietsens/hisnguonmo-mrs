
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisMediReactSumViewFilter : FilterBase
    {
        public long? TREATMENT_ID { get; set; }
        public List<long> TREATMENT_IDs { get; set; }

        public HisMediReactSumViewFilter()
            : base()
        {
        }
    }
}


using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisImpMestUserViewFilter : FilterBase
    {
        public long? IMP_MEST_ID { get; set; }
        public List<long> IMP_MEST_IDs { get; set; }

        public HisImpMestUserViewFilter()
            : base()
        {
        }
    }
}

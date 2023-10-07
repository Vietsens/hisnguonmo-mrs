
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisImpMestUserFilter : FilterBase
    {
        public long? IMP_MEST_ID { get; set; }
        public List<long> IMP_MEST_IDs { get; set; }

        public HisImpMestUserFilter()
            : base()
        {
        }
    }
}

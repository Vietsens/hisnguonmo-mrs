using System;
using System.Collections.Generic;
using System.Text;

namespace MOS.Filter
{
    public class HisAggrImpMestFilter : FilterBase
    {
        public List<long> IMP_MEST_IDs { get; set; }
        public long? IMP_MEST_ID { get; set; }

        public HisAggrImpMestFilter()
            : base()
        {
        }
    }
}

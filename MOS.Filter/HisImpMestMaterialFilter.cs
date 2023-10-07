
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisImpMestMaterialFilter : FilterBase
    {
        public List<long> MATERIAL_IDs { get; set; }
        public List<long> IMP_MEST_IDs { get; set; }
        public long? MATERIAL_ID { get; set; }
        public long? IMP_MEST_ID { get; set; }

        public long? IMP_MEST_ID__NOT__EQUAL { get; set; }

        public HisImpMestMaterialFilter()
            : base()
        {
        }
    }
}

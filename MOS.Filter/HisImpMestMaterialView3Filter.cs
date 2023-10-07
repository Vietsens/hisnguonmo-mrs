
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisImpMestMaterialView3Filter : FilterBase
    {
        public List<long> MATERIAL_IDs { get; set; }
        public long? MATERIAL_ID { get; set; }
        public long? IMP_MEST_ID { get; set; }
        public long? MATERIAL_TYPE_ID { get; set; }
        public List<long> IMP_MEST_IDs { get; set; }
        public List<long> MATERIAL_TYPE_IDs { get; set; }
        public long? IMP_TIME_FROM { get; set; }
        public long? IMP_TIME_TO { get; set; }

        public HisImpMestMaterialView3Filter()
            : base()
        {
        }
    }
}

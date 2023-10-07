using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisExpMestMaterialView3Filter : FilterBase
    {
        public List<long> EXP_MEST_IDs { get; set; }
        public List<long> MATERIAL_IDs { get; set; }
        public long? MATERIAL_ID { get; set; }
        public long? EXP_MEST_ID { get; set; }
        public long? MATERIAL_TYPE_ID { get; set; }
        public long? EXP_TIME_FROM { get; set; }
        public long? EXP_TIME_TO { get; set; }

        public HisExpMestMaterialView3Filter()
            : base()
        {
        }
    }
}

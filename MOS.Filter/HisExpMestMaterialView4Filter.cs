using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisExpMestMaterialView4Filter : FilterBase
    {
        public List<long> EXP_MEST_IDs { get; set; }
        public List<long> MATERIAL_IDs { get; set; }
        public List<long> TDL_MATERIAL_TYPE_IDs { get; set; }
        public List<long> EXP_MEST_TYPE_IDs { get; set; }

        public long? MATERIAL_ID { get; set; }
        public long? EXP_MEST_ID { get; set; }
        public long? TDL_MATERIAL_TYPE_ID { get; set; }
        public long? EXP_TIME_FROM { get; set; }
        public long? EXP_TIME_TO { get; set; }
        public long? EXP_MEST_TYPE_ID { get; set; }

        public HisExpMestMaterialView4Filter()
            : base()
        {
        }
    }
}

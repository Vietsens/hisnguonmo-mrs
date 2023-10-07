using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisExpMestMaterialFilter : FilterBase
    {
        public long? EXP_MEST_ID { get; set; }
        public long? MATERIAL_ID { get; set; }
        public long? TDL_AGGR_EXP_MEST_ID { get; set; }
        public long? TDL_AGGR_EXP_MEST_ID__OR__EXP_MEST_ID { get; set; }
        public List<long> TDL_AGGR_EXP_MEST_IDs { get; set; }
        public List<long> EXP_MEST_IDs { get; set; }
        public List<long> MATERIAL_IDs { get; set; }

        public bool? IS_EXPORT { get; set; }
        public bool? IS_APPROVED { get; set; }

        public long? PATIENT_TYPE_ID { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }

        public HisExpMestMaterialFilter()
            : base()
        {
        }
    }
}

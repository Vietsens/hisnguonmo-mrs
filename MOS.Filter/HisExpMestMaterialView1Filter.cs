using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisExpMestMaterialView1Filter : FilterBase
    {
        public List<long> EXP_MEST_IDs { get; set; }
        public List<long> MATERIAL_IDs { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public long? MATERIAL_ID { get; set; }
        public long? EXP_MEST_ID { get; set; }
        public long? MATERIAL_TYPE_ID { get; set; }
        public long? MEDI_STOCK_ID { get; set; }

        public HisExpMestMaterialView1Filter()
            : base()
        {
        }
    }
}


using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisExpMestMatyFilter : FilterBase
    {
        public long? EXP_MEST_ID { get; set; }
        public List<long> EXP_MEST_IDs { get; set; }
        public long? MATERIAL_TYPE_ID { get; set; }

        public HisExpMestMatyFilter()
            : base()
        {
        }
    }
}

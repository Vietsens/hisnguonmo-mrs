
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisExpMestMetyFilter : FilterBase
    {
        public long? EXP_MEST_ID { get; set; }
        public List<long> EXP_MEST_IDs { get; set; }
        public long? MEDICINE_TYPE_ID { get; set; }

        public HisExpMestMetyFilter()
            : base()
        {
        }
    }
}

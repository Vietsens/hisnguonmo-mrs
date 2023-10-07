
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisExpMestBltyViewFilter : FilterBase
    {
        public long? EXP_MEST_ID { get; set; }
        public List<long> EXP_MEST_IDs { get; set; }

        public HisExpMestBltyViewFilter()
            : base()
        {
        }
    }
}


using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisExpMestUserViewFilter : FilterBase
    {
        public long? EXP_MEST_ID { get; set; }
        public List<long> EXP_MEST_IDs { get; set; }

        public long? EXECUTE_ROLE_ID { get; set; }
        public List<long> EXECUTE_ROLE_IDs { get; set; }

        public HisExpMestUserViewFilter()
            : base()
        {
        }
    }
}

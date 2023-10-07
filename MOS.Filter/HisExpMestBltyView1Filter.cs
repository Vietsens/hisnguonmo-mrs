
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisExpMestBltyView1Filter : FilterBase
    {
        public long? EXP_MEST_ID { get; set; }
        public List<long> EXP_MEST_IDs { get; set; }

        public HisExpMestBltyView1Filter()
            : base()
        {
        }
    }
}

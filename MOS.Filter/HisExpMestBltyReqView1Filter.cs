
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisExpMestBltyReqView1Filter : FilterBase
    {
        public long? EXP_MEST_ID { get; set; }
        public List<long> EXP_MEST_IDs { get; set; }
        public long? BLOOD_TYPE_ID { get; set; }
        public List<long> BLOOD_TYPE_IDs { get; set; }

        public HisExpMestBltyReqView1Filter()
            : base()
        {
        }
    }
}

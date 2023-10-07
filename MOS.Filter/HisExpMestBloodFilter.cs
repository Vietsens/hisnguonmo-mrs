
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisExpMestBloodFilter : FilterBase
    {
        public List<long> EXP_MEST_IDs { get; set; }
        public List<long> BLOOD_IDs { get; set; }
        public long? EXP_MEST_ID { get; set; }
        public long? BLOOD_ID { get; set; }
        public bool? IS_EXPORT { get; set; }

        public HisExpMestBloodFilter()
            : base()
        {
        }
    }
}

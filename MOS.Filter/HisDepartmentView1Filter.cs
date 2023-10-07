
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisDepartmentView1Filter : FilterBase
    {
        public List<long> BRANCH_IDs { get; set; }
        public long? BRANCH_ID { get; set; }
        public bool? IS_CLINICAL { get; set; }

        public HisDepartmentView1Filter()
            : base()
        {
        }
    }
}

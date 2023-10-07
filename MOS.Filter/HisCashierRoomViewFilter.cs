
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisCashierRoomViewFilter : FilterBase
    {
        public long? BRANCH_ID { get; set; }
        public List<long> BRANCH_IDs { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; }

        public HisCashierRoomViewFilter()
            : base()
        {
        }
    }
}

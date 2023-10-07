
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisExecuteRoomViewFilter : FilterBase
    {
        public bool? IS_EXAM { get; set; }
        public long? BRANCH_ID { get; set; }
        public List<long> BRANCH_IDs { get; set; }

        public HisExecuteRoomViewFilter()
            : base()
        {
        }
    }
}

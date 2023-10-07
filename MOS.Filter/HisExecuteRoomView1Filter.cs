
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisExecuteRoomView1Filter : FilterBase
    {
        public List<long> BRANCH_IDs { get; set; }

        public bool? IS_EXAM { get; set; }
        public long? BRANCH_ID { get; set; }

        public HisExecuteRoomView1Filter()
            : base()
        {
        }
    }
}

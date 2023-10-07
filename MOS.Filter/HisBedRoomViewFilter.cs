
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisBedRoomViewFilter : FilterBase
    {
        public long? DEPARTMENT_ID { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; }

        public HisBedRoomViewFilter()
            : base()
        {
        }
    }
}

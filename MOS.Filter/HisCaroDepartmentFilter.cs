
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisCaroDepartmentFilter : FilterBase
    {
        public long? CASHIER_ROOM_ID { get; set; }
        public List<long> CASHIER_ROOM_IDs { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; }

        public HisCaroDepartmentFilter()
            : base()
        {
        }
    }
}

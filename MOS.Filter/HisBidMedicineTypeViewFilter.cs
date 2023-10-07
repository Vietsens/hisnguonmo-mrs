
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisBidMedicineTypeViewFilter : FilterBase
    {
        public List<long> MEDICINE_TYPE_IDs { get; set; }
        public List<long> BID_IDs { get; set; }
        public long? MEDICINE_TYPE_ID { get; set; }
        public long? BID_ID { get; set; }

        public HisBidMedicineTypeViewFilter()
            : base()
        {
        }
    }
}

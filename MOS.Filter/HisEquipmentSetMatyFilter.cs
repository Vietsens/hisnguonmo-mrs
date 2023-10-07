
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisEquipmentSetMatyFilter : FilterBase
    {

        public long? EQUIPMENT_SET_ID { get; set; }
        public long? MATERIAL_TYPE_ID { get; set; }

        public List<long> EQUIPMENT_SET_IDs { get; set; }
        public List<long> MATERIAL_TYPE_IDs { get; set; }

        public HisEquipmentSetMatyFilter()
            : base()
        {
        }
    }
}

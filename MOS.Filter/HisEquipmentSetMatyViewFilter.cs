
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisEquipmentSetMatyViewFilter : FilterBase
    {
        public string MATERIAL_TYPE_CODE__EXACT { get; set; }

        public long? EQUIPMENT_SET_ID { get; set; }
        public long? MATERIAL_TYPE_ID { get; set; }
        public long? SERVICE_ID { get; set; }
        public long? SERVICE_UNIT_ID { get; set; }

        public List<long> EQUIPMENT_SET_IDs { get; set; }
        public List<long> MATERIAL_TYPE_IDs { get; set; }
        public List<long> SERVICE_IDs { get; set; }
        public List<long> SERVICE_UNIT_IDs { get; set; }

        public HisEquipmentSetMatyViewFilter()
            : base()
        {
        }
    }
}

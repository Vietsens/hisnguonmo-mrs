using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisMaterialBeanFilter : FilterBase
    {
        public long? MATERIAL_ID { get; set; }
        public long? MEDI_STOCK_ID { get; set; }
        public List<long> MATERIAL_IDs { get; set; }
        public List<long> MATERIAL_TYPE_IDs { get; set; }
        public List<string> DETACH_KEYs { get; set; }
        public short? MATERIAL_IS_ACTIVE { get; set; }

        public HisMaterialBeanFilter()
            : base()
        {
        }
    }
}

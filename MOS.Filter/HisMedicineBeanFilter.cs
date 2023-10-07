using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisMedicineBeanFilter : FilterBase
    {
        public long? MEDICINE_ID { get; set; }
        public long? MEDI_STOCK_ID { get; set; }
        public List<long> MEDICINE_IDs { get; set; }
        public List<long> MEDICINE_TYPE_IDs { get; set; }
        public string DETACH_KEY__EXACT { get; set; }
        public List<string> DETACH_KEYs { get; set; }
        public short? MEDICINE_IS_ACTIVE { get; set; }

        public HisMedicineBeanFilter()
            : base()
        {
        }
    }
}

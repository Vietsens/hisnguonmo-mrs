using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisMedicineBeanView1Filter : FilterBase
    {
        public long? MEDI_STOCK_ID {get;set;}
        public long? MEDICINE_TYPE_ID { get; set; }
        public long? MEDICINE_TYPE_IS_ACTIVE { get; set; }
        public long? MEDICINE_IS_ACTIVE { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public List<long> MEDICINE_TYPE_IDs { get; set; }
        public List<long> MEDICINE_IDs { get; set; }
        public List<long> SOURCE_IDs { get; set; }
        public long? MEDICINE_ID { get; set; }

        public HisMedicineBeanView1Filter()
            : base()
        {
        }
    }
}

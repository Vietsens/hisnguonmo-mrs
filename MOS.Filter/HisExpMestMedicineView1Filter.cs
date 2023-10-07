using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisExpMestMedicineView1Filter : FilterBase
    {
        public List<long> EXP_MEST_IDs { get; set; }
        public List<long> MEDICINE_IDs { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }

        public long? EXP_MEST_ID { get; set; }
        public long? MEDICINE_TYPE_ID { get; set; }
        public long? MEDI_STOCK_ID { get; set; }
        public long? MEDICINE_ID { get; set; }
        public List<long> TDL_SERVICE_REQ_IDs { get; set; }
        public HisExpMestMedicineView1Filter()
            : base()
        {
        }
    }
}

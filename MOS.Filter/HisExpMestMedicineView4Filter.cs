using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisExpMestMedicineView4Filter : FilterBase
    {
        public List<long> EXP_MEST_IDs { get; set; }
        public List<long> MEDICINE_IDs { get; set; }
        public List<long> TDL_MEDICINE_TYPE_IDs { get; set; }
        public List<long> EXP_MEST_TYPE_IDs { get; set; }

        public long? EXP_MEST_ID { get; set; }
        public long? TDL_MEDICINE_TYPE_ID { get; set; }
        public long? EXP_MEST_TYPE_ID { get; set; }
        public long? MEDICINE_ID { get; set; }
        public long? EXP_TIME_FROM { get; set; }
        public long? EXP_TIME_TO { get; set; }
        public List<long> TDL_SERVICE_REQ_IDs { get; set; }

        public HisExpMestMedicineView4Filter()
            : base()
        {
        }
    }
}

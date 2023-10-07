
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisPrescriptionView3Filter : FilterBase
    {
        public List<long> MEDI_STOCK_IDs { get; set; }
        public List<long> EXP_MEST_IDs { get; set; }
        public List<long> SERVICE_REQ_IDs { get; set; }
        public List<long> EXP_MEST_STT_IDs { get; set; }

        public long? EXP_MEST_ID { get; set; }
        public long? SERVICE_REQ_ID { get; set; }
        public long? TREATMENT_ID { get; set; }
        public bool? HAS_AGGR { get; set; }

        public long? EXP_MEST_STT_ID { get; set; }
        public long? MEDI_STOCK_ID { get; set; }
        public long? USE_TIME__TO { get; set; }
        public long? USE_TIME__FROM { get; set; }

        public string EXP_MEST_CODE__EXACT { get; set; }

        public HisPrescriptionView3Filter()
            : base()
        {
        }
    }
}

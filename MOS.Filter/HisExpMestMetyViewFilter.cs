
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisExpMestMetyViewFilter : FilterBase
    {
        public List<long> EXP_MEST_IDs { get; set; }
        public long? MEDI_STOCK_ID { get; set; }
        public long? EXP_TIME_FROM { get; set; }
        public long? EXP_TIME_TO { get; set; }
        public long? EXP_MEST_ID { get; set; }
        public long? MEDI_STOCK_PERIOD_ID { get; set; }
        public bool? HAS_MEDI_STOCK_PERIOD { get; set; }
        public long? MEDICINE_TYPE_ID { get; set; }
        public long? EXP_MEST_STT_ID { get; set; }
        public long? EXP_MEST_TYPE_ID { get; set; }
        public long? AGGR_EXP_MEST_ID { get; set; }
        public string EXP_MEST_CODE__EXACT { get; set; }

        public HisExpMestMetyViewFilter()
            : base()
        {
        }
    }
}

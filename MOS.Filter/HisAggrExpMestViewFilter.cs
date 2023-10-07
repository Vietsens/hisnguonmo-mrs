
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisAggrExpMestViewFilter : FilterBase
    {
        public long? EXP_MEST_ID { get; set; }
        public long? REQ_DEPARTMENT_ID { get; set; }
        public long? REQ_ROOM_ID { get; set; }
        public long? EXP_MEST_STT_ID { get; set; }
        public List<long> EXP_MEST_STT_IDs { get; set; }
        public string EXP_MEST_CODE__EXACT { get; set; }
        public long? EXP_TIME_FROM { get; set; }
        public long? EXP_TIME_TO { get; set; }

        public HisAggrExpMestViewFilter()
            : base()
        {
        }
    }
}

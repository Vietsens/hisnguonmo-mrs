
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisBedLogFilter : FilterBase
    {
        public List<long> TREATMENT_BED_ROOM_IDs { get; set; }
        public List<long> SERVICE_REQ_IDs { get; set; }
        public List<long> BED_IDs { get; set; }

        public long? TREATMENT_BED_ROOM_ID { get; set; }
        public long? SERVICE_REQ_ID { get; set; }
        public long? BED_ID { get; set; }

        public long? START_TIME_FROM { get; set; }
        public long? FINISH_TIME_FROM { get; set; }
        public long? START_TIME_TO { get; set; }
        public long? FINISH_TIME_TO { get; set; }
        public bool? IS_FINISH { get; set; }
        
        public HisBedLogFilter()
            : base()
        {
        }
    }
}

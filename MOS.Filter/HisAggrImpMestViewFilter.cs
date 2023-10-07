using System;
using System.Collections.Generic;
using System.Text;

namespace MOS.Filter
{
    public class HisAggrImpMestViewFilter : FilterBase
    {
        public long? REQ_DEPARTMENT_ID { get; set; }
        public long? REQ_ROOM_ID { get; set; }
        public long? IMP_MEST_ID { get; set; }
        public List<long> IMP_MEST_IDs { get; set; }
        public List<long> IMP_MEST_STT_IDs { get; set; }
        public long? IMP_MEST_STT_ID { get; set; }
        public long? IMP_TIME_FROM { get; set; }
        public long? IMP_TIME_TO { get; set; }

        public HisAggrImpMestViewFilter()
            : base()
        {
        }
    }
}

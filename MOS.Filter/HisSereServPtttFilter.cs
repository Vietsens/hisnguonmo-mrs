using System;
using System.Collections.Generic;
using System.Text;

namespace MOS.Filter
{
    public class HisSereServPtttFilter : FilterBase
    {
        public long? BLOOD_ID { get; set; }
        public long? BLOOD_RH_ID { get; set; }
        public long? EMOTIONLESS_METHOD_ID { get; set; }
        public long? PTTT_CATASTROPHE_ID { get; set; }
        public long? PTTT_CONDITION_ID { get; set; }
        public long? PTTT_GROUP_ID { get; set; }
        public long? PTTT_METHOD_ID { get; set; }
        public List<long> SERE_SERV_IDs { get; set; }
        public List<long> PTTT_GROUP_IDs { get; set; }

        public HisSereServPtttFilter()
            : base()
        {
        }
    }
}

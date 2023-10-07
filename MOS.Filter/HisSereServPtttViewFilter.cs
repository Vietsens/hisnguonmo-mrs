using System;
using System.Collections.Generic;
using System.Text;

namespace MOS.Filter
{
    public class HisSereServPtttViewFilter : FilterBase
    {
        public List<long> TDL_TREATMENT_IDs { get; set; }
        public List<long> SERE_SERV_IDs { get; set; }
        public List<long> PTTT_GROUP_IDs { get; set; }

        public long? SERE_SERV_ID { get; set; }
        public long? PTTT_GROUP_ID { get; set; }

        public HisSereServPtttViewFilter()
            : base()
        {
        }
    }
}

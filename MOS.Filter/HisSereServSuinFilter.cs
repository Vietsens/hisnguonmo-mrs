using System;
using System.Collections.Generic;
using System.Text;

namespace MOS.Filter
{
    public class HisSereServSuinFilter : FilterBase
    {
        public List<long> SERE_SERV_IDs { get; set; }
        public long? SUIM_INDEX_ID { get; set; }

        public HisSereServSuinFilter()
            : base()
        {
        }
    }
}

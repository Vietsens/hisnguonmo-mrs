using System;
using System.Collections.Generic;
using System.Text;

namespace MOS.Filter
{
    public class HisSereServSuinViewFilter : FilterBase
    {
        public long? SERE_SERV_ID { get; set; }
        public List<long> SERE_SERV_IDs { get; set; }
        public List<long> SUIN_INDEX_IDs { get; set; }
        
        public HisSereServSuinViewFilter()
            : base()
        {
        }
    }
}

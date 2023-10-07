using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisSereServTeinFilter : FilterBase
    {
        public List<long> SERE_SERV_IDs { get; set; }
        public long? TEST_INDEX_ID { get; set; }

        public HisSereServTeinFilter()
            : base()
        {
        }
    }
}

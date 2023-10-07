using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisSereServTeinViewFilter : FilterBase
    {
        public long? SERE_SERV_ID { get; set; }
        public List<long> SERE_SERV_IDs { get; set; }
        public List<long> TDL_TREATMENT_IDs { get; set; }
        public List<string> TEST_INDEX_CODEs { get; set; }

        public HisSereServTeinViewFilter()
            : base()
        {
        }
    }
}


using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisSereServExtFilter : FilterBase
    {
        public long? SERE_SERV_ID { get; set; }
        public List<long> SERE_SERV_IDs { get; set; }

        public long? BEGIN_TIME_FROM { get; set; }
        public long? BEGIN_TIME_TO { get; set; }
        public long? END_TIME_FROM { get; set; }
        public long? END_TIME_TO { get; set; }
        public bool? IS_NOT_FEE { get; set; }
        public bool? IS_NOT_GATHER_DATA { get; set; }

        public HisSereServExtFilter()
            : base()
        {
        }
    }
}

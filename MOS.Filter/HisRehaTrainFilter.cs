
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisRehaTrainFilter : FilterBase
    {
        public long? SERE_SERV_REHA_ID { get; set; }
        public List<long> SERE_SERV_REHA_IDs { get; set; }

        public HisRehaTrainFilter()
            : base()
        {
        }
    }
}

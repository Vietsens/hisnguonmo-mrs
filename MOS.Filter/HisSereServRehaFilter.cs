
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisSereServRehaFilter : FilterBase
    {
        public List<long> SERE_SERV_IDs { get; set; }
        public long? SERE_SERV_ID { get; set; }
        public long? REHA_TRAIN_TYPE_ID { get; set; }

        public HisSereServRehaFilter()
            : base()
        {
        }
    }
}

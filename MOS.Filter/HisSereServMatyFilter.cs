
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisSereServMatyFilter : FilterBase
    {
        public long? SERE_SERV_ID { get; set; }
        public long? MATERIAL_TYPE_ID { get; set; }

        public List<long> SERE_SERV_IDs { get; set; }
        public List<long> MATERIAL_TYPE_IDs { get; set; }

        public HisSereServMatyFilter()
            : base()
        {
        }
    }
}

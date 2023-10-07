
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisSereServRehaViewFilter : FilterBase
    {
        public List<long> SERE_SERV_IDs { get; set; }
        public List<long> SERVICE_REQ_IDs { get; set; }
        public long? SERVICE_REQ_ID { get; set; }

        public HisSereServRehaViewFilter()
            : base()
        {
        }
    }
}

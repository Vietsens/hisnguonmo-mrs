
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisRemunerationViewFilter : FilterBase
    {
        public long? SERVICE_ID { get; set; }
        public long? EXECUTE_ROLE_ID { get; set; }
        public List<long> SERVICE_IDs { get; set; }
        public List<long> EXECUTE_ROLE_IDs { get; set; }

        public HisRemunerationViewFilter()
            : base()
        {
        }
    }
}

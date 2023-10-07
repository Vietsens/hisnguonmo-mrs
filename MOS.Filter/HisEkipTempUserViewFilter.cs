
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisEkipTempUserViewFilter : FilterBase
    {
        public List<long> EKIP_TEMP_IDs { get; set; }
        public List<long> EXECUTE_ROLE_IDs { get; set; }

        public long? EKIP_TEMP_ID { get; set; }
        public long? EXECUTE_ROLE_ID { get; set; }

        public HisEkipTempUserViewFilter()
            : base()
        {
        }
    }
}

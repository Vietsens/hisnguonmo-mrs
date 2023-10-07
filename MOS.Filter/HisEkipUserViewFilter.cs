
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisEkipUserViewFilter : FilterBase
    {
        public List<long> EKIP_IDs { get; set; }
        public List<long> EXECUTE_ROLE_IDs { get; set; }
        public long? EKIP_ID { get; set; }
        public long? EXECUTE_ROLE_ID { get; set; }
        
        public HisEkipUserViewFilter()
            : base()
        {
        }
    }
}

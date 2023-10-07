
using System.Collections.Generic;
namespace SDA.Filter
{
    public class SdaConfigAppUserFilter : FilterBase
    {
        public string LOGINNAME { get; set; }
        public long? CONFIG_APP_ID { get; set; }
        public List<long> CONFIG_APP_IDs { get; set; }
        public SdaConfigAppUserFilter()
            : base()
        {
        }
    }
}

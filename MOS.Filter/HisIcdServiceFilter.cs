
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisIcdServiceFilter : FilterBase
    {
        public long? SERVICE_ID { get; set; }

        public List<long> ICD_IDs { get; set; }
        public List<long> SERVICE_IDs { get; set; }
        public string ICD_CODE__EXACT { get; set; }
        public List<string> ICD_CODE__EXACTs { get; set; }

        public HisIcdServiceFilter()
            : base()
        {
        }
    }
}

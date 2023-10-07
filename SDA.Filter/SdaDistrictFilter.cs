using System.Collections.Generic;

namespace SDA.Filter
{
    public class SdaDistrictFilter : FilterBase
    {
        public List<long> PROVINCE_IDs { get; set; }

        public SdaDistrictFilter()
            : base()
        {
        }
    }
}

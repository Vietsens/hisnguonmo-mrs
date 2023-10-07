using System.Collections.Generic;

namespace SDA.Filter
{
    public class SdaProvinceFilter : FilterBase
    {
        public List<long> NATIONAL_IDs { get; set; }

        public SdaProvinceFilter()
            : base()
        {
        }
    }
}

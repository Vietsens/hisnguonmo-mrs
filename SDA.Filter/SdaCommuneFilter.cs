using System.Collections.Generic;

namespace SDA.Filter
{
    public class SdaCommuneFilter : FilterBase
    {
        public List<long> DISTRICT_IDs { get; set; }

        public SdaCommuneFilter()
            : base()
        {
        }
    }
}

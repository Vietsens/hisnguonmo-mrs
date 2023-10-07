using System;
using System.Collections.Generic;
using System.Text;

namespace MOS.Filter
{
    public class HisInfusionViewFilter : FilterBase
    {
        public List<long> INFUSION_SUM_IDs { get; set; }
        public List<long> MEDICINE_IDs { get; set; }
        public long? INFUSION_SUM_ID { get; set; }
        public long? MEDICINE_ID { get; set; }

        public HisInfusionViewFilter()
            : base()
        {
        }
    }
}

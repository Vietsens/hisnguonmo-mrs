using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisMediReactViewFilter : FilterBase
    {
        public List<long> MEDICINE_IDs { get; set; }
        public List<long> MEDI_REACT_SUM_IDs { get; set; }

        public long? MEDI_REACT_TYPE_ID { get; set; }
        public long? MEDICINE_ID { get; set; }
        public long? MEDI_REACT_SUM_ID { get; set; }

        public HisMediReactViewFilter()
            : base()
        {
        }
    }
}

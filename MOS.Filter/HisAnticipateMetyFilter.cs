
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisAnticipateMetyFilter : FilterBase
    {
        public long? ANTICIPATE_ID { get; set; }
        public long? MEDICINE_TYPE_ID { get; set; }
        public List<long> MEDICINE_TYPE_IDs { get; set; }
        public List<long> ANTICIPATE_IDs { get; set; }

        public HisAnticipateMetyFilter()
            : base()
        {
        }
    }
}

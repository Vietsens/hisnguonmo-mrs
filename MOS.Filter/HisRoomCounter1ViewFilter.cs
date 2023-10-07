
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisRoomCounter1ViewFilter
    {
        protected static readonly long NEGATIVE_ID = -1;

        public long? ID { get; set; }
        public List<long> IDs { get; set; }

        public string ORDER_FIELD { get; set; }
        public string ORDER_DIRECTION { get; set; }
        
        public HisRoomCounter1ViewFilter()
            : base()
        {

        }
    }
}

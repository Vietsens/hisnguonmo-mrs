
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisBillFundFilter : FilterBase
    {
        public List<long> BILL_IDs { get; set; }
        public long? BILL_ID { get; set; }

        public HisBillFundFilter()
            : base()
        {
        }
    }
}

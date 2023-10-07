
using System.Collections.Generic;
namespace HTC.Filter
{
    public class HtcExpenseViewFilter : FilterBase
    {
        public long? PERIOD_ID { get; set; }

        public long? EXPENSE_TYPE_ID { get; set; }

        public List<long> PERIOD_IDs { get; set; }

        public List<long> EXPENSE_TYPE_IDs { get; set; }

        public long? EXPENSE_TIME_FROM { get; set; }

        public long? EXPENSE_TIME_TO { get; set; }

        public HtcExpenseViewFilter()
            : base()
        {
        }
    }
}

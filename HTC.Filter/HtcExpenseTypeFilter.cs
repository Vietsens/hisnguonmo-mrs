
namespace HTC.Filter
{
    public class HtcExpenseTypeFilter : FilterBase
    {
        public long? PARENT_ID { get; set; }

        public bool? IsAllowExpense { get; set; }

        public HtcExpenseTypeFilter()
            : base()
        {
        }
    }
}


namespace MOS.Filter
{
    public class HisAccountBookFilter : FilterBase
    {
        public bool? FOR_DEPOSIT { get; set; }
        public bool? FOR_REPAY { get; set; }
        public bool? FOR_BILL { get; set; }

        public HisAccountBookFilter()
            : base()
        {

        }
    }
}

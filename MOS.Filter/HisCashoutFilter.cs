
namespace MOS.Filter
{
    public class HisCashoutFilter : FilterBase
    {
        public long? CASHOUT_TIME_FROM { get; set; }
        public long? CASHOUT_TIME_TO { get; set; }

        public HisCashoutFilter()
            : base()
        {
        }
    }
}

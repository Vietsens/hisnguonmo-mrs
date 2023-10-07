
namespace MOS.Filter
{
    public class HisEmployeeFilter : FilterBase
    {
        public string LOGINNAME__EXACT { get; set; }
        public long? ID__NOT_EQUAL { get; set; }

        public HisEmployeeFilter()
            : base()
        {
        }
    }
}

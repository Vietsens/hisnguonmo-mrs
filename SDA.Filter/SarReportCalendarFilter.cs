
namespace SDA.Filter
{
    public class SarReportCalendarFilter : FilterBase
    {
        public enum ExecuteEnum
        {
            EXECUTED,
            NOT_EXECUTE,
        }
        public ExecuteEnum? EXECUTE { get; set; }

        /// <summary>
        /// Thoi gian quet bao cao.
        /// Phuc vu truy van cac bao cao co EXECUTE_TIME nho hon SCAN_TIME nhung chua duoc xu ly.
        /// </summary>
        public long? SCAN_TIME { get; set; }

        public SarReportCalendarFilter()
            : base()
        {
        }
    }
}

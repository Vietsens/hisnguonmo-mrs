using System.Collections.Generic;

namespace SDA.Filter
{
    public class SarReportViewFilter : FilterBase
    {
        public enum ReadModeEnum
        {
            CREATE,
            RECEIVE,
            PUBLIC,
            ALL,
        }
        /// <summary>
        /// Che do doc (vi sao duoc xem)
        /// CREATE - Do minh tao ra
        /// RECEIVE - Do minh nhan duoc
        /// PUBLIC - Do duoc cong cong (khong bao gom 2 loai tren)
        /// NULL - Tong hop cua 3 dieu kien tren
        /// </summary>
        public ReadModeEnum READ_MODE { get; set; }
        public List<long> REPORT_STT_IDs { get; set; }

        public SarReportViewFilter()
            : base()
        {
        }
    }
}

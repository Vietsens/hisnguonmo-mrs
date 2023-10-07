using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00064
{
    /// <summary>
    /// Báo cáo thống kê chi tiết xuất sử dụng theo từng khoa phòng - MSS
    /// </summary>
    class Mrs00064Filter
    {
        // Khoa phòng bắt buộc truyền vào phòng của khoa
        public long? DEPARTMENT_ID { get; set; }
        public long? ROOM_ID { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; }
        public List<long> ROOM_IDs { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }

        public long? PATIENT_TYPE_ID { get; set; }
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00064Filter()
            : base()
        {
        }
    }
}

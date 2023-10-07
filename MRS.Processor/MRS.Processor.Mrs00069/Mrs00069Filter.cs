using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00069
{
    /// <summary>
    /// Báo cáo tổng hợp xuất sử dụng cho đối tượng điều trị bảo hiểm y tế
    /// </summary>
    class Mrs00069Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        //Bắt buộc truyền vào kho, nếu không truyền MEDI_STOCK_ID = 0=> khong get được dữ liệu
        public long MEDI_STOCK_ID { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }

        public Mrs00069Filter()
            : base()
        {
        }
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00151
{
    //	Báo cáo Tổng hợp tất cả số lượng thuốc nhận hoàn trả từ Khoa,phòng
    public class Mrs00151Filter
    {
        public long DEPARTMENT_ID { get;  set;  }

        public long? DEPARTMENT_ROOM_ID { get;  set;  }//không bắt buộc nhập

        public long DATE_FROM { get;  set;  }

        public long DATE_TO { get;  set;  }
    }
}

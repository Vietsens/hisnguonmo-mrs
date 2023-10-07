using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00345
{
    public class Mrs00345Filter
    {
        public long TIME_FROM { get;  set;  }                         // thời gian chỉ định
        public long TIME_TO { get;  set;  }

        public List<long> DEPARTMENT_IDs { get;  set;  }              // khoa phòng xử lý (khám)
        public List<long> SERVICE_REQ_STT_IDs { get;  set;  }         // tình trạng khám
        public List<long> PATIENT_TYPE_IDs { get;  set;  }            // đối tượng bệnh nhân(bhyt, vp, ...)
        public bool NEW_REGISTRATION { get;  set;  }                  // đăng ký mới
        public bool APPOINTMENT { get;  set;  }                       // hẹn khám
        public bool TRANSFER_ROOM { get;  set;  }                     // chuyển phòng khám
        public bool TRANSFER_CLINICAL { get;  set;  }                 // chuyển từ khoa lâm sàng


        public List<long> EXECUTE_ROOM_IDs{ get; set; }                //PHÒNG KHÁM
        public List<long> EXACT_BED_ROOM_IDs { set; get; }              //buồng điều trị
    }
}

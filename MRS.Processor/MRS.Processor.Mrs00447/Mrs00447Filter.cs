using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00447
{
    public class Mrs00447Filter
    {
        public long TIME_FROM { get;  set;  }                 // thời gian nhập kho
        public long TIME_TO { get;  set;  }

        public bool? IS_ECG { get;  set;  }                    // điện tim
        public bool? IS_NEB { get;  set;  }                    // khí dung

        public List<long> EXE_ROOM_IDs { get; set; }    // phòng thực hiện
        public List<long> EXECUTE_ROOM_IDs { get; set; }    // phòng thực hiện
        public long? REQUEST_TREATMENT_TYPE_ID { get; set; }
        public List<long> REQUEST_TREATMENT_TYPE_IDs { get; set; }

        public long? TREATMENT_TYPE_ID { get; set; }
        public long? REQUEST_DEPARTMENT_ID { get; set; }
        public long? EXECUTE_DEPARTMENT_ID { get; set; }
        public long? REQUEST_ROOM_ID { get; set; }

        public List<long> PATIENT_TYPE_IDs { get; set; }//đối tượng thanh toán
        public List<long> TDL_PATIENT_TYPE_IDs { get; set; }//đối tượng bệnh nhân
        public List<long> MACHINE_IDs { get; set; }//máy cấu hình trong dịch vụ
        public List<long> EXECUTE_MACHINE_IDs { get; set; }//máy chọn ở phòng xử lý
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00255
{
    public class Mrs00255Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public long? DEPARTMENT_ID { get;  set;  }
        public List<long> DEPARTMENT_IDs { get;  set;  }

        public long? PATIENT_TYPE_ID { get;  set;  } //đối tượng bệnh nhân
        public long? TREATMENT_TYPE_ID { get;  set;  }

        public long? TDL_PATIENT_TYPE_ID { get; set; }// đối tượng thanh toán
        public List<long> TDL_PATIENT_TYPE_IDs { get; set; }// đối tượng thanh toán

        public List<long> EXECUTE_ROOM_IDs { get; set; }
        public short? IS_PT_TT { get; set; }//null:all; 1:PT; 0: TT
        public bool? IS_END_TIME { get; set; }//thoi gian loc: true-ket thuc;false-bat dau;null-chi dinh
        public bool? IS_PAUSE { get; set; }//da ket thuc dieu tri
        public bool? IS_LOCK_FEE { get; set; }//da khoa vien phi
        public List<long> SERVICE_REQ_STT_IDs { get; set; } // trạng thái y lệnh
        //public bool? IS_DETAIL_TREATMENT { get; set; }//chi tiet ma dieu tri
        public List<long> SERVICE_IDs { get; set; }
        public List<long> SERVICE_TYPE_IDs { get; set; }
        public long? SERVICE_TYPE_ID { get; set; }

        public long? SERVICE_ID { get; set; }

        public long? INPUT_DATA_ID_TIME_TYPE { get; set; }//1. vào viện. 2. chỉ định. 3. bắt đầu y lệnh. 4. kết thúc y lệnh. 5. ra viện. 6. thanh toán. 7. khóa viện phí. 8. bắt đầu xử lý
        public List<long> EXECUTE_DEPARTMENT_IDs { get; set; }
        public string KEY_GROUP_MEMA_FOLLOW { get; set; }

        public string KEY_ORDER { get; set; }
    }
}

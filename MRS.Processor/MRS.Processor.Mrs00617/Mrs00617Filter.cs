using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00617
{
    public class Mrs00617Filter
    {
        public long? TIME_FROM { get;  set;  }
        public long? TIME_TO { get;  set;  }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        
        //đối tượng thanh toán
        public List<long> PATIENT_TYPE_IDs { get; set; }
        //khoa phòng thực hiện
        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }
        public List<long> REQUEST_ROOM_IDs { get; set; }
        public List<long> EXECUTE_DEPARTMENT_IDs { get; set; }
        public List<long> EXECUTE_ROOM_IDs { get; set; }
        //loại dịch vụ, nhóm dịch vụ, dịch vụ
        public List<long> SERVICE_TYPE_IDs { get; set; }
        public List<long> EXACT_PARENT_SERVICE_IDs { get; set; }
        public List<long> EXACT_CHILD_SERVICE_IDs { get; set; }
        public List<long> SERVICE_REQ_STT_IDs { get; set; }
        public string SVT_LIMIT_CODE { get; set; }

        public List<long> TDL_PATIENT_TYPE_IDs { get; set; }//đối tượng bệnh nhân

        public long? INPUT_DATA_ID_TIME_TYPE { get; set; }
        public bool? PATIENT_TYPE_IS_THU_PHI { get; set; }

        public List<long> MACHINE_IDs { get; set; }//máy cấu hình trong dịch vụ
        public List<long> EXECUTE_MACHINE_IDs { get; set; }//máy chọn ở phòng xử lý

        public bool? IS_AMOUNT { get; set; }//Đếm theo số lượng dịch vụ

        public List<long> BRANCH_IDs { get; set; }
        public long? BRANCH_ID { get; set; }
    }
}

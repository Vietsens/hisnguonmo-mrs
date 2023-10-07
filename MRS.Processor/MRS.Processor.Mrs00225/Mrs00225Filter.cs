using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00225
{
    public class Mrs00225Filter
    {
        public long FINISH_TIME_FROM { get;  set;  }
        public long FINISH_TIME_TO { get;  set;  }
        public List<long> PTTT_GROUP_IDs { get;  set;  }
        public long? EXECUTE_DEPARTMENT_ID { get;  set;  }

        public long? TREATMENT_TYPE_ID { get;  set;  }
        public List<long> REQUEST_DEPARTMENT_IDs { get; set; } //khoa chỉ định
        public List<long> EXECUTE_DEPARTMENT_IDs { get; set; } //Khoa thực hiện
        public List<long> PATIENT_TYPE_IDs { get; set; } //đối tượng thanh toán
        public List<string> REQUEST_LOGINNAMEs { get; set; } //bác sỹ chỉ định
        public List<long> EXECUTE_ROOM_IDs { get; set; } //phòng thực hiện
        public long? INPUT_DATA_ID_TIME_TYPE { get;  set; }
    }
}

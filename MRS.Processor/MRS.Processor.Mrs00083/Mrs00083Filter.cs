using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00083
{
    public class Mrs00083Filter : FilterBase
    {
        public long? BRANCH_ID { get; set; }
        public List<long> BRANCH_IDs { get; set; }

        public long? TIME_FROM { get;  set;  }
        public long? TIME_TO { get;  set;  }
        public long? FINISH_TIME_FROM { get;  set;  }
        public long? FINISH_TIME_TO { get;  set;  }

        public long? REQUEST_ROOM_ID { get; set; }
        public long? REQUEST_DEPARTMENT_ID { get; set; }
        public long? EXECUTE_ROOM_ID { get; set; }
        public long? EXECUTE_DEPARTMENT_ID { get; set; }

        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }

        public List<long> EXECUTE_DEPARTMENT_IDs { get; set; }

        public List<long> REQUEST_ROOM_IDs { get; set; }

        public List<long> EXECUTE_ROOM_IDs { get; set; }

      
        public List<long> MACHINE_IDs { get; set; }
        public List<long> EXECUTE_MACHINE_IDs { get; set; } //Lọc mã máy theo thông tin nhập ở phòng xử lý

        public long? TREATMENT_TYPE_ID { get; set; }
        public long? REQUEST_TREATMENT_TYPE_ID { get; set; }
        public List<long> REQUEST_TREATMENT_TYPE_IDs { get; set; }
        //public bool? IS_START_TIME { get; set; }
        public Mrs00083Filter()
            : base()
        {

        }

        public long? INPUT_DATA_ID_TIME_TYPE { get; set; }

        public List<long> PATIENT_TYPE_IDs { get; set; }

        public List<long> TDL_PATIENT_TYPE_IDs { get; set; }

        public long? REPORT_TYPE_CAT_ID { get; set; }

        public List<long> REPORT_TYPE_CAT_IDs { get; set; }


    }
}

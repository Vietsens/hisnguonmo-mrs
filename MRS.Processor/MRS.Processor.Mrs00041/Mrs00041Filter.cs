using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00041
{
    public class Mrs00041Filter 
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public long? FINISH_TIME_FROM { get; set; }
        public long? FINISH_TIME_TO { get; set; }
        public long? REQUEST_DEPARTMENT_ID { get; set; }
        public long? EXECUTE_DEPARTMENT_ID { get; set; }
        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }
        public List<long> EXECUTE_DEPARTMENT_IDs { get; set; }
        public long? TREATMENT_TYPE_ID { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public long? REPORT_TYPE_CAT_ID { get; set; }
        public List<long> REPORT_TYPE_CAT_IDs { get; set; }
        public bool? IS_MERGE_TREATMENT { get; set; }
        public string KEY_MERGE_TREATMENT { get; set; }
        public bool? IS_MERGE_INT_PAR { get; set; }
        public bool? IS_START_TIME { get; set; }
        public List<long> SERVICE_IDs { get; set; }
        public List<long> EXACT_PARENT_SERVICE_IDs { get; set; }
        public List<long> SERVICE_FULL_IDs { get; set; }

        public List<long> BRANCH_IDs { get; set; }
        public Mrs00041Filter()
            : base()
        {

        }

        public long? INPUT_DATA_ID_TIME_TYPE { get; set; }

        public long? EXECUTE_ROOM_ID { get; set; }

        public List<long> PATIENT_TYPE_IDs { get; set; } //ĐỐI TƯỢNG THANH TOÁN

        public List<long> TDL_PATIENT_TYPE_IDs { get; set; } //ĐỐI TƯỢNG BỆNH NHÂN
        public bool? IS_ADD_GP { get; set; }
        public bool? IS_USER_FROM_ACS { get; set; }

        public List<long> MACHINE_IDs { get; set; } //Lọc máy theo cấu hình ở dịch vụ kỹ thuật

        public List<long> EXECUTE_MACHINE_IDs { get; set; } //Lọc mã máy theo thông tin nhập ở phòng xử lý
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00484
{
    public class Mrs00484Filter
    {
        public List<long> BRANCH_IDs { get; set; }
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public long? BRANCH_ID { get; set; }
        public List<long> REPORT_TYPE_CAT_IDs { get; set; }
        public long? REPORT_TYPE_CAT_ID { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public bool? IS_MERGE_TREATMENT { get; set; }
        public short? CHOOSE_TREATMENT_TYPE { get; set; }

        public long? EXECUTE_ROOM_ID { get; set; }

        public bool? IS_FINISH { get; set; }
        //them dieu kien loc khong gop dich vu cua benh nhan
        public bool? IS_SPLIT_SV { get; set; }

        public long? EXECUTE_DEPARTMENT_ID { get; set; }
        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }
        public List<long> EXECUTE_DEPARTMENT_IDs { get; set; }
        public long? TREATMENT_TYPE_ID { get; set; }

        public long? INPUT_DATA_ID_TIME_TYPE { get; set; }

        public bool? IS_NOT_SHOW_ALL { get; set; }

        public List<long> PATIENT_TYPE_IDs { get; set; }//đối tượng thanh toán
        public List<long> TDL_PATIENT_TYPE_IDs { get; set; }//đối tượng bệnh nhân
        public List<long> MACHINE_IDs { get; set; }//máy cấu hình trong dịch vụ
        public List<long> EXECUTE_MACHINE_IDs { get; set; }//máy chọn ở phòng xử lý
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00042
{
    public class Mrs00042Filter
    {
        public long? BRANCH_ID { get; set; }
        public List<long> BRANCH_IDs { get; set; }
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public long? FINISH_TIME_FROM { get; set; }
        public long? FINISH_TIME_TO { get; set; }
        public short? INPUT_DATA_ID_TIME_TYPE { get; set; } //1:vao vien, 2:chi dinh, 3:bat dau, 4:ket thuc, 5: ra vien, 6: thanh toan, 7: khoa vien phi, 8: kết quả
        public long? REQUEST_DEPARTMENT_ID { get; set; }
        public long? EXECUTE_DEPARTMENT_ID { get; set; }
        public long? REQUEST_ROOM_ID { get; set; }
        public List<long> EXECUTE_ROOM_IDs { get; set; }
        public long? EXE_ROOM_ID { get; set; }
        public long? TREATMENT_TYPE_ID { get; set; }
        public long? REQUEST_TREATMENT_TYPE_ID { get; set; }
        public List<long> REQUEST_TREATMENT_TYPE_IDs { get; set; }
        public List<long> REPORT_TYPE_CAT_IDs { get; set; }
        public List<long> SERVICE_TYPE_IDs { get; set; }//loại dịch vụ
        public List<long> EXACT_PARENT_SERVICE_IDs { get; set; }
        public string SVT_LIMIT_CODE { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }//đối tượng thanh toán
        public List<long> PATIENT_TYPE_IDs { get; set; }//đối tượng thanh toán
        public List<long> TDL_PATIENT_TYPE_IDs { get; set; }//đối tượng bệnh nhân
        public List<long> MACHINE_IDs { get; set; }//máy cấu hình trong dịch vụ
        public List<long> EXECUTE_MACHINE_IDs { get; set; }//máy chọn ở phòng xử lý
        public string DE_AREAs { get; set; }
        public bool? IS_START_TIME { get; set; }
        public List<long> SERVICE_IDs { get; set; }
        public short? INPUT_DATA_ID_BILL_STATUS { get; set; }//trạng thái thanh toán: 1: Đã thanh toán; 2: Chưa thanh toán
        public List<long> TREATMENT_TYPE_IDs { get; set; }//diện điều trị
        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }//khoa chỉ định
        public string REQUEST_LOGINNAME { get; set; }//bác sĩ chỉ định
        public string SUBCLINICAL_RESULT_LOGINNAME { get; set; }//bác sĩ đọc kết quả

        public Mrs00042Filter()
            : base()
        {

        }

        public bool? IS_ADD_NULL_PARENT { get; set; }
    }
}

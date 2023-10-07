using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00714
{
    public class Mrs00714Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; }
        public List<long> SERVICE_TYPE_IDs { get; set; }
        public List<long> SERVICE_IDs { get; set; }
        public List<long> EXECUTE_ROOM_IDs { get; set; }
        public List<long> EXECUTE_DEPARTMENT_IDs { get; set; }
        public List<long> TDL_PATIENT_TYPE_IDs { get; set; }
        public List<long> REQ_DEPARTMENT_IDs { get; set; }
        public List<string> LOGINNAMEs { get; set; }
        public short? INPUT_DATA_ID_TIME_TYPE { get; set; } // Loại thời gian
        public bool? ADD_EKIP_NO_REMU { get; set; } //thêm ekip không có phụ thu
        public string ADD_MAIN_ROLEs { get; set; } //thêm vai trò chính
        public string ADD_SUPPORT_ROLEs { get; set; } //thêm vai trò phụ
        public string ADD_HELPER_ROLEs { get; set; } //thêm vai giúp việc
        public short? INPUT_DATA_ID_STT_PAUSE { get; set; } //Trạng thái RV: 1:Ra viện,2:chưa ra viện
        public short? INPUT_DATA_ID_STT_PAY { get; set; } //Trạng thái TT: 1:đã thanh toán,2:chưa thanh toán

        public List<long> REPORT_TYPE_CAT_IDs { get; set; }

        public List<long> EXCLUDE_SERVICE_IDs { get; set; }

        public short? IS_PT_TT { get; set; }//null:all; 1:PT; 0: TT

        public string PTTT_GROUP_CODE__PTs { get; set; }

        public string PTTT_GROUP_CODE__TTs { get; set; }

        public string PATIENT_CLASSIFY_CODE__TMs { get; set; }
        public bool? IS_GATHER_DATA { get; set; } //Chỉ lấy dữ liệu đã chốt
        public bool? IS_REMU_PRICE_WITH_EKIP { get; set; } //Lấy dữ liệu phụ cấp theo ekip
    }
}

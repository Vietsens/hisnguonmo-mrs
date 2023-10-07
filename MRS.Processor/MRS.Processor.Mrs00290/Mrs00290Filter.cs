using MOS.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00290
{
    public class Mrs00290Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public List<string> LOGINNAMEs { get; set; }
        public List<string> LOGINNAME_DOCTORs { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public long? SERVICE_TYPE_ID { get; set; }
        public List<long> SERVICE_TYPE_IDs { get; set; }
        public List<long> SERVICE_IDs { get; set; }

        public long? REQUEST_DEPARTMENT_ID { get; set; }

        public long? EXECUTE_DEPARTMENT_ID { get; set; }

        public long? EXECUTE_ROOM_ID { get; set; }

        public string KEY_GROUP_SS { get; set; }

        public long? INPUT_DATA_ID_TIME_TYPE { get; set; }

        public bool? IS_NOT_TAKE_AMOUNT_SV { get; set; }

        public List<long> SERVICE_REQ_STT_IDs { get; set; }

        public List<long> TDL_PATIENT_TYPE_IDs { get; set; }

        public List<long> PATIENT_TYPE_IDs { get; set; }

        public List<long> PATIENT_CLASSIFY_IDs { get; set; }

        public long? TDL_PATIENT_TYPE_ID { get; set; }

        public long? SERVICE_REQ_STT_ID { get; set; }

        public long? PATIENT_CLASSIFY_ID { get; set; }

        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }

        public List<long> EXECUTE_ROOM_IDs { get; set; }

        public List<long> EXECUTE_DEPARTMENT_IDs { get; set; }

        public long? REQUEST_ROOM_ID { get; set; }

        public List<long> REQUEST_ROOM_IDs { get; set; }

        public long? SERVICE_ID { get; set; }

        public long? EXACT_PARENT_SERVICE_ID { get; set; }

        public List<long> EXACT_PARENT_SERVICE_IDs { get; set; }

        public long? TREATMENT_TYPE_ID { get; set; }

        public long? EXACT_CASHIER_ROOM_ID { get; set; }

        public List<long> EXACT_CASHIER_ROOM_IDs { get; set; }

        public string CASHIER_LOGINNAME { get; set; }

        public List<string> CASHIER_LOGINNAMEs { get; set; }

        public string SERVICE_NAME { get; set; }

        public List<long> ICD_IDs { get; set; }

        public short? INPUT_DATA_ID_EXPEND_TYPE { get; set; } //1:hao phi; 2:khong hao phi

        public string PAY_RATE_RATIO { get; set; }

        public List<long> EXAM_ROOM_IDs { get; set; }

        public List<long> LAST_DEPARTMENT_IDs { get; set; }

        public List<long> BRANCH_IDs { get; set; }

        public List<long> REPORT_TYPE_CAT_IDs { get; set; }

        public short? INPUT_DATA_ID_FEE_TYPE { get; set; } //1:BHYT; 2:Tự trả. Khi chọn bảo hiểm thì lấy chi phí đối tượng bảo hiểm, không tính tiền phụ thu. Khi chọn tự trả thì lấy chi phí của đối tượng khác bảo hiểm và phần tiền phụ thu của đối tượng bảo hiểm.
    }
}

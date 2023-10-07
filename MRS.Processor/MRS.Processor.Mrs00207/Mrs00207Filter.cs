using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00207
{
    public class Mrs00207Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; }
        public long? EXAM_ROOM_ID { get; set; }
        public List<long> EXAM_ROOM_IDs { get; set; }
        public long? MEDICINE_TYPE_ID { get; set; }
        public List<long> MEDICINE_TYPE_IDs { get; set; }
        public long? TREATMENT_TYPE_ID { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public long? MEDI_STOCK_ID { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public long? EXP_MEST_TYPE_ID { get; set; }
        public List<long> EXP_MEST_TYPE_IDs { get; set; }
        public bool? ADD_SALE { get; set; }

        public bool? TRUE_FALSE { get; set; }//Thời gian y lệnh: sai;

        public long? INPUT_DATA_ID_TIME_TYPE { get; set; }//Loại thời gian: 1: Chỉ định; 2: Xuất; 3: Vào viện; 4: Ra viện; 5: Khóa viện phí; 6: Thanh toán

        public long? REPORT_TYPE_CAT_ID { get; set; }
        public List<long> REPORT_TYPE_CAT_IDs { get; set; }
        public List<long> MEDICINE_GROUP_IDs { get; set; }
        public List<string> DOCTOR_LOGINNAMEs { get; set; }
        public string KEY_GROUP_EXMM { get; set; }
        public bool? IS_MATERIAL { get; set; }
        public bool? IS_MEDICINE { get; set; }
        public List<long> EXACT_PARENT_SERVICE_IDs { get; set; }

        public bool? ADD_PAR_SERVICE { get; set; }

        public bool? ADD_REQ_BED { get; set; }//thêm giường kê đơn

        public List<long> EMPLOYEE_DEPARTMENT_IDs { get; set; }//Khoa nhân viên ở

        public List<string> ICD_CODEs { get; set; }
    }
}

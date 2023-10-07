using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00198
{
    public class VSarReportMrs00198RDO
    {
        public string PATIENT_CODE { get; set; }//mã BN
        public string PATIENT_NAME { get; set; }//tên BN
        public string GENDER_NAME_MALE { get; set; }//ngày sinh BN nam
        public string GENDER_NAME_FEMALE { get; set; }//ngày sinh BN nữ
        public string VIR_ADRESS { get; set; }//đia chỉ
        public string PHONE_NUMBER { get; set; }//SĐT
        public string ICD_CODE { get; set; }//mã bệnh
        public string ICD_NAME { get; set; }//tên bệnh chính
        public string ICD_TEXT { get; set; }//tên bệnh phụ
        public string ICD_NAME_MAIN_EXAMINATION { get; set; }//bệnh chính lần khám trước
        public string TIME_EXAMINATION_FIRST { get; set; }//ngày khám trước
        public string TOTAL_DATE_STANDBY { get; set; }//tổng thời gian từ ngày khám đến thời gian hẹn khám sau
        public string APPOINTMENT_TIME { get; set; }
        public string END_DEPARTMENT_NAME { get; set; }
        public string APPOINTMENT_ROOM_NAME { get; set; }
        public string APPOINTMENT_DEPARTMENT_NAME { get; set; }
        public string EXECUTE_ROOM_NAME { get; set; }
        public string DOCTOR_USERNAME { get; set; }
        public string PATIENT_UNSIGNED_NAME { get; set; }
        public string END_ROOM_NAME { get; set; }
        public long IN_TIME { get; set; }
        public long? CLINICAL_IN_TIME { get; set; }
        public long? OUT_TIME { get; set; }
    }
}
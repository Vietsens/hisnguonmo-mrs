using MOS.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00556
{
    public class Mrs00556Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public long? TREATMENT_TYPE_ID { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public List<long> TDL_PATIENT_TYPE_IDs { get; set; }
        public List<string> CASHIER_LOGINNAMEs { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; }
        public string CATEGORY_CODE__ECG { get; set; }
        public string CATEGORY_CODE__EEG { get; set; }
        public string CATEGORY_CODE__BRAIN_BLOOD { get; set; }
        public string CATEGORY_CODE__CERVICAL_ENDO { get; set; }
        public List<string> SERVICE_CODE__TRANs { get; set; }
        public List<string> SERVICE_CODE__KHAMSKs { get; set; }

        public string SERVICE_CODE__Z { get; set; }
        public string SERVICE_CODE__P { get; set; }
        public string CATEGORY_CODE__NT { get; set; }
        public string CATEGORY_CODE__HH { get; set; }
        public string CATEGORY_CODE__VS { get; set; }
        public string CATEGORY_CODE__SH { get; set; }
        public string SERVICE_CODE__GKSK { get; set; }
        public string CATEGORY_CODE__BONE_DENSITY { get; set; }
        public string CATEGORY_CODE__NSTH { get; set; }
        public string CATEGORY_CODE__CLVT { get; set; }
        public string CATEGORY_CODE__XQ { get; set; }
        public string CATEGORY_CODE__CNHH { get; set; }
        public string CATEGORY_CODE__GYC { get; set; } //GIƯỜNG YC
        public string CATEGORY_CODE__CLVTYC { get; set; } //CT - SCANNER YC
        public string CATEGORY_CODE__KYC { get; set; } //KHÁM YC
        public string CATEGORY_CODE__XNGX { get; set; } // XÉT NGHIỆM GENEXPERT

        //chi lay dich vu thu phi cua bn bao hiem
        public bool? IS_FEE_OF_BHYT { get; set; }
        public long? EXAM_ROOM_ID { get; set; }
        public List<long> EXAM_ROOM_IDs { get; set; }
        public bool? REDU_OVER_HEIN_LIMIT { get; set; }
        public int INPUT_DATA_ID_TIME_TYPE { get; set; }
        public string KEY_GROUP_SS { get; set; }
        public bool? EXAM_REQ_USER_IS_NOT_EXAM_EXE_USER { get; set; }
        //

        public string CATEGORY_CODE__HIV { get; set; } //HIV

        public string CATEGORY_CODE__GPB { get; set; } //giải phẫu bệnh

        public string CATEGORY_CODE__TrM { get; set; } //Máu truyền

        public string CATEGORY_CODE__XQKTS { get; set; } //XQ KTS

        public string CATEGORY_CODE__SAG { get; set; } //Siêu âm gan

        public string CATEGORY_CODE__DIGEST_ENDO { get; set; } //Nội soi tiêu hóa

        public string CATEGORY_CODE__TMH_ENDO { get; set; } //Nội soi tai mũi họng

        public string CATEGORY_CODE__DTD { get; set; } //Điện tâm đồ

        public string CATEGORY_CODE__DCD { get; set; } //Điện cơ đồ

        public string CATEGORY_CODE__ABI { get; set; } //ABI

        public string CATEGORY_CODE__DTDGS { get; set; } //Điện tâm đồ gắng sức

        public string CATEGORY_CODE__TNT { get; set; } //Thận nhân tạo

        public List<long> SERVICE_TYPE_IDs { get; set; }

        public List<long> REQUEST_ROOM_IDs { get; set; }

        public int? INPUT_DATA_ID_STT_TYPE { get; set; }//1: đã ra viện ; 2: chưa ra viện

    }
}
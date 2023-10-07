using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00382
{
    public class Mrs00382Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public long? MEDI_STOCK_ID { get;  set;  } //kho xuất
        public List<long> IMP_MEDI_STOCK_IDs { get; set; } //kho nhập
        public List<long> EXP_MEDI_STOCK_IDs { get; set; } //kho xuất
        public long? EXP_MEST_TYPE_ID { get; set; } //loại xuất
        public List<long> EXP_MEST_TYPE_IDs { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; } //khoa
        public List<long> EXAM_ROOM_IDs { get; set; } //phòng khám
        public List<long> REQ_ROOM_IDs { set; get; }// phòng yc
        public long? IMP_MEST_TYPE_ID { get; set; } //loại nhập
        public List<long> MEDICINE_GROUP_IDs { get; set; }
        public List<long> SERVICE_IDs { get; set; }
        public List<long> SERVICE_TYPE_IDs { get; set; } //giới hạn chỉ lấy thuốc và vật tư
        public List<long> CHEMICAL_SUBSTANCE_TYPE_IDs { get; set; } //hóa chất
        public List<string> DOCTOR_LOGINNAMEs { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public List<long> SS_PATIENT_TYPE_IDs { get; set; }
        public string PATIENT_NAMEs { get; set; }
        public List<long> PATIENT_CLASSIFY_IDs { set; get; }
        public List<long> TREATMENT_TYPE_IDs { set; get; }
        public string ICD_CODE { set; get; }
        public bool? IS_FEE_LOCK { get; set; }
        public List<long> EXP_MEST_REASON_IDs { set; get; }
        public string KEY_GROUP_EXP { get; set; }// key gộp chi tiết phiếu xuất

        public List<long> INPUT_DATA_ID_MEMA_TYPEs { get; set; }//1:Thuốc,2:Vật tư
        public bool IS_GROUP { set; get; }// gộp các phiếu xuất cùng 1 ngườu lấy

        public List<long> MEDI_STOCK_CABINET_IDs { get; set; }

        public string KEY_GROUP_TEMP_TYPEs { get; set; } //danh sách các name manager sẽ lấy vào báo cáo.

        public List<long> OTHER_PAY_SOURCE_IDs { get; set; }//nguồn chi trả khác

        public short? INPUT_DATA_ID_BSCS_TYPE { get; set; } // lọc theo loại bổ sung cơ số: 1. Thêm bổ sung cơ số, 2. Bớt bổ sung cơ số, 3. Chỉ lấy bổ sung cơ số
    }
}

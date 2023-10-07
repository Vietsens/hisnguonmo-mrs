using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00662
{
    public class Mrs00662Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }

        public long? ONLY_MONTH_FROM { get; set; }
        public long? ONLY_MONTH_TO { get; set; }

        public short? IS_NOT_FEE { get; set; }//null:all; 1:khong lay; 0:co
        public short? IS_NOT_GATHER_DATA { get; set; }//null:all; 1:khong lay; 0:co

        public short? IS_PT_TT { get; set; }//null:all; 1:PT; 0: TT
        public short? IS_NT_NGT { get; set; }//null:all; 1:NT; 0:NGT
        public short? IS_NT_NGT_0 { get; set; }//null:all; 1:NT; 0:NGT

        public List<long> EXECUTE_DEPARTMENT_IDs { get; set; }
        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }

        public List<long> EXECUTE_ROOM_IDs { get; set; }
        public List<long> REQUEST_ROOM_IDs { get; set; }

        public string PTTT_GROUP_CODE__TTs { get; set; }

        public string PTTT_GROUP_CODE__PTs { get; set; }
        public bool? HAS_EKIP { get; set; }

        public List<long> PATIENT_TYPE_IDs { get; set; }

        public List<long> TDL_PATIENT_TYPE_IDs { get; set; }
        public long? PATIENT_CLASSIFY_ID { get; set; }
        public bool? IS_SPLIT_PT_TT { get; set; }
        public bool? IS_PTTM { set; get; }
        public bool? ADD_DEBATE { set; get; }
        public short? INPUT_DATA_ID_STT_PAY { get; set; } //Trạng thái TT: 1:đã thanh toán,2:chưa thanh toán
        public short? INPUT_DATA_ID_STT_PAUSE { get; set; } //Trạng thái RV: 1:Ra viện,2:chưa ra viện
        public short? INPUT_DATA_ID_TIME_TYPE { get; set; } // Loại thời gian
        public short? INPUT_DATA_ID_TIME_TYPE_PTTM { get; set; } // Loại thời gian cho đối tượng phẫu thuật thẩm mỹ
        public bool? TAKE_MONTH { get; set; } //lấy theo tháng
        public List<long> SERVICE_IDs { get; set; }
        public List<long> SERVICE_TYPE_IDs { get; set; }
        public List<long> EXCLUDE_SERVICE_IDs { get; set; }
        public string SERVICE_CODE__HCs { get; set; } //"SERVICE_CODE__HCs":{0}_OUTPUT0:TT13.KB001,TT13.KB002,TT13.KB003,TT13.KB004,TT13.KB005,TT13.KB006,TT13.KB007,TT13.KB008,TT13.KB009,TT13.KB010,TT13.KB011,TT13.KB012,TT13.KB013,TT13.KB014,TT13.KB015,TT13.KB016,TT37.KB016
        public string SERVICE_CODE__PTYCs { get; set; }
        public long? INPUT_DATA_ID__TIME_TYPE { get; set; }

        public long? TREATMENT_TYPE_IDs { get; set; }

        public List<long> REPORT_TYPE_CAT_IDs { get; set; }

        public string PATIENT_CLASSIFY_CODE__TMs { get; set; }
        public short? INPUT_DATA_ID_FEE_TYPE { get; set; } //1:BHYT; 2:Tự trả. Khi chọn bảo hiểm thì lấy chi phí đối tượng bảo hiểm, không tính tiền phụ thu. Khi chọn tự trả thì lấy chi phí của đối tượng khác bảo hiểm và phần tiền phụ thu của đối tượng bảo hiểm.



        public List<long> EXCLUDE_EXECUTE_DEPARTMENT_IDs { get; set; }
    }
}

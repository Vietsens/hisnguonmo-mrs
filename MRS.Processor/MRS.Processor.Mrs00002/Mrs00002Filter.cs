using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00002
{
    public class Mrs00002Filter
    {
        public long? CREATE_TIME_FROM { get;  set;  }
        public long? CREATE_TIME_TO { get;  set;  }
        public long? INTRUCTION_TIME_FROM { get;  set;  }
        public long? INTRUCTION_TIME_TO { get; set; }

        public List<long> EXAM_ROOM_IDs { get; set; }

        public List<long> DEPARTMENT_IDs { get; set; }
        /// <summary>
        /// các mã khoa luôn tính là khám, không tính nội trú, 
        /// nếu dữ liệu thực tế chuyển bệnh nhân vào điều trị nội trú ở các khoa này
        /// thì báo cáo sẽ không tính nội trú mà tính đang khám ở phòng khám thuộc khoa
        /// </summary>
        public string DEPARTMENT_CODE__OUTPATIENTs { get; set; } 
        public bool? IS_INTRUCTION_TIME_OR_FINISH_TIME { get; set; } //Lọc theo: null: thời gian chỉ định, true: thời gian chỉ định, false: thời gian kết thúc
        public List<long> TREATMENT_TYPE_IDs { get; set; }//diện điều trị
        public bool? IS_BILL_TIME { get; set; }
        public bool? IS_NOT_CHECK_IN_TREAT { get; set; }//Không check yêu cầu nhập viện
        public List<long> TDL_PATIENT_TYPE_IDs { get; set; }//đối tượng bệnh nhân
        public bool? IS_HOLIDAYS { get; set; }
        public bool? IS_NOT_HOLIDAYS { get; set; }

        public bool? KEY_GET_TOGETHER_A1 { get; set; }//Lấy đồng thời dữ liệu A1 và xử lý

        public Mrs00002Filter()
            : base()
        {
        }

        public bool? BANNGAY_IS_NOITRU { get; set; }

        public short? INPUT_DATA_ID_STT_TYPE { get; set; }//Trạng thái điều trị: 1. Đã kết thúc, 2. Chưa kết thúc, 3. Tất cả

        public List<long> BRANCH_IDs { get; set; }

        public bool? IS_COUNT_REQ { get; set; }
    }
}

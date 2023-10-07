using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00040
{
    public class Mrs00040Filter
    {
        public long? BRANCH_ID { get; set; }
        public List<long> BRANCH_IDs { get; set; }
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public long? FINISH_TIME_FROM { get; set; }
        public long? FINISH_TIME_TO { get; set; }
        public List<long> REQUEST_ROOM_IDs { get; set; }
        public long? REQUEST_DEPARTMENT_ID { get; set; }
        public long? EXECUTE_DEPARTMENT_ID { get; set; }
        public long? EXECUTE_ROOM_ID { get; set; }
        public List<long> EXECUTE_ROOM_IDs { get; set; }
        public List<long> MY_SURG_ROOM_IDs { get; set; }
        public long? TREATMENT_TYPE_ID { get; set; }
        public bool? HAS_BILL { get; set; }
        public bool? HAS_BILL_OR_BHYT { get; set; }
        public List<long> FINAL_PATIENT_TYPE_IDs { get; set; }

        public short? IS_NOT_FEE { get; set; }//null:all; 1:khong lay; 0:co
        public short? IS_NOT_GATHER_DATA { get; set; }//null:all; 1:khong lay; 0:co

        public short? IS_NT_NGT { get; set; }//null:all; 1:NT; 0:NGT
        public short? IS_NT_NGT_0 { get; set; }//null:all; 1:NT; 0:NGT
        /// <summary>
        /// day of week
        /// 1: monday
        /// 2: tuesday
        /// 3: wednesday
        /// 4: thursday
        /// 5: friday
        /// 6: saturday
        /// 7: sunday
        /// </summary>
        public List<long> INPUT_DATA_IDs { get; set; }
        public List<long> SERVICE_TYPE_IDs { get; set; }
        public List<long> SERVICE_IDs { get; set; }

        public string PTTT_GROUP_CODE__TTs { get; set; }

        public short? INPUT_DATA_ID_TIME_TYPE { get; set; } //1:vao vien,2:chi dinh,3:bat dau,4:ket thuc,5:ra vien,6:thanh toan,7:khoa vien phi,8:thuc hien
        public List<long> EXECUTE_DEPARTMENT_IDs { get; set; }
        public List<long> REPORT_TYPE_CAT_IDs { get; set; }
        public bool? ADD_REAL_METHOD_INFO { get; set; }
        public short? IS_COOK_MEDICINE { set; get; }


        public List<long> PATIENT_TYPE_IDs { get; set; } //ĐỐI TƯỢNG THANH TOÁN

        public List<long> TDL_PATIENT_TYPE_IDs { get; set; } //ĐỐI TƯỢNG BỆNH NHÂN

        public List<long> MACHINE_IDs { get; set; } //Lọc máy theo cấu hình ở dịch vụ kỹ thuật

        public List<long> EXECUTE_MACHINE_IDs { get; set; } //Lọc mã máy theo thông tin nhập ở phòng xử lý

        public Mrs00040Filter()
            : base()
        {

        }
    }
}

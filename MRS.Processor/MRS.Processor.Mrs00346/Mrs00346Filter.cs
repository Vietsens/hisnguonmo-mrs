using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00346
{
    public class Mrs00346Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public long? BRANCH_ID { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }// đối tượng bệnh nhân
        public long? SERE_SERV_PATIENT_TYPE_ID { get; set; }//đối tượng thanh toán
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public short? TIME_TYPE { get; set; }// null:duyet bhyt  | 0: vao vien | 1: ra vien |2: khoa vp
        /// <summary>
        /// True: chi lay noi tru
        /// False: chi lay ngoai tru
        /// Null: lay ca hai
        /// </summary>
        public bool? IS_TREAT { get; set; }
        /// <summary>
        /// True: chi lay da ket thuc
        /// False: chi lay chua ket thuc
        /// Null: lay ca hai
        /// </summary>
        public bool? IS_ACTIVE { get; set; }
        /// <summary>
        /// True: chi lay da khoa vp
        /// False: chi lay chua khoa vp
        /// Null: lay ca hai
        /// </summary>
        public bool? IS_PAUSE { get; set; }

        public bool? HAS_PRIMARY_PATIENT_TYPE_ID { get; set; }
        public List<long> FEE_LOCK_ROOM_IDs { get; set; }
        public List<string> FEE_LOCK_LOGINNAMEs { get; set; }
        //loại dịch vụ, nhóm dịch vụ, dịch vụ
        public List<long> SERVICE_TYPE_IDs { get; set; }
        public List<long> EXACT_PARENT_SERVICE_IDs { get; set; }
        public List<long> EXACT_CHILD_SERVICE_IDs { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }// đối tượng bệnh nhân
        public List<long> SERE_SERV_PATIENT_TYPE_IDs { get; set; }//đối tượng thanh toán
        public string IS_FILTER_NOT_BHYT { get; set; }// lọc chỉ lấy bệnh nhân tự trả
        public bool? IS_SHOW_ROOM { get; set; }

        public short? INPUT_DATA_ID_STT_TYPE { get; set; } //trạng thái bệnh nhân: 1: Đang điều trị; 2: Đã kết thúc; 3: Đã khóa viện phí; 4: Đã duyệt BHYT

        public List<long> LAST_DEPARTMENT_IDs { get; set; }
    }
}

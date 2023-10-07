using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00021
{
    /// <summary>
    /// De nghi thanh toan bao hiem y te benh nhan kham chua benh noi tru c80a
    /// </summary>
    public class Mrs00021Filter
    {
        public long? BRANCH_ID { get; set; }
        public long? DEPARTMENT_ID { get; set; }

        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public bool? IS_MERGE_TREATMENT { get; set; }

        public long? OUT_TIME_FROM { get; set; }
        public long? OUT_TIME_TO { get; set; }
        public List<long> END_DEPARTMENT_IDs { get; set; }
        public List<long> END_ROOM_IDs { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public string KEY_GROUP_TREA { get; set; }

        public Mrs00021Filter()
            : base()
        {

        }

        public List<long> BRANCH_IDs { get; set; }
        public List<long> TREATMENT_END_TYPE_IDs { get; set; }

        //public bool? IS_ADD_ACCUMM_TREA { get; set; }// Thêm dữ liệu tích lũy lượt khám

        public List<long> SERVICE_TYPE_IDs { get; set; }
        public List<long> SERVICE_IDs { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }// đối tượng thanh toán
        public short? INPUT_DATA_ID_PROVINCE_TYPE { get; set; } //1:Noi tinh, 2:Ngoai tinh

        public short? INPUT_DATA_ID_ROUTE_TYPE { get; set; } //1:Dung tuyen, 2:Trai tuyen
        public short? INPUT_DATA_ID_TIME_TYPE { set; get; }//1 vào viện,2 ra viện,3 khóa viện phí, 4 duyệt giám định bhxh

        public bool? IS_SPLIT_DEPA { get; set; }

        public string ACCEPT_HEIN_MEDI_ORG_CODE { get; set; }
    }
}

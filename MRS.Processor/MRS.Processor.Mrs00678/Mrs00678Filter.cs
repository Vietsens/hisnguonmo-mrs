using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00678
{
    public class Mrs00678Filter
    {
        public long? EXAM_ROOM_ID { get; set; }
        public long? REQUEST_ROOM_ID { get; set; }
        public List<long> EXAM_ROOM_IDs { get; set; }
        public List<long> REQUEST_ROOM_IDs { get; set; }
        public long? FINISH_TIME_FROM { get; set; }
        public long? FINISH_TIME_TO { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public bool? IS_FINISH { get; set; }
        public bool? IS_MERGE_EXAM_ROOM { get; set; }
        public List<long> TDL_PATIENT_TYPE_IDs { get; set; }
        public string DEPARTMENT_CODE__OUTPATIENTs { get; set; }

        public short? INPUT_DATA_ID_TIME_TYPE { get; set; } //1:vao vien,2:chi dinh,3:bat dau,4:ket thuc,5:ra vien//,6:thanh toan,7:khoa vien phi,8:thuc hien

        public bool? IS_ELDER { get; set; } // checkbox là người già

        public bool? IS_TREATIN { get; set; } // checkbox là nội trú

        public long? BRANCH_ID { get; set; }
    }
}

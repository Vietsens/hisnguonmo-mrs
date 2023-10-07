using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00476
{
    public class Mrs00476Filter
    {
        public long TIME_FROM { get;  set;  }             // thời gian khóa vp
        public long TIME_TO { get;  set;  }

        public List<long> EXECUTE_ROOM_IDs { get; set; }// phòng lấy bc
        public List<long> EXAM_ROOM_IDs { get; set; }// phòng khám lấy bc
        public string LOGINNAME_DOCTOR { get; set; }// bác sĩ khám
        public List<long> ROOM_IDs { get; set; }// phòng lấy bc
        /// <summary>
        /// type of examination finish
        /// 1: Has prescription in room
        /// 2: Add examination in room
        /// 3: speciality examination in room
        /// 4: tranfer hopital in room
        /// </summary>
        public List<long> INPUT_DATA_IDs { get; set; }

        public bool? EXAM_END_TYPE__PAUSE { get; set; } //Khám kết thúc điều trị

        public bool? EXAM_END_TYPE__INTREAT { get; set; } //Khám nhập viện

        public bool? EXAM_EXECUTING { get; set; } //Đang khám

        public bool? EXAM_YET { get; set; } //Đang khám
        public List<long> AREA_IDs { get; set; }// phòng khám lấy bc
    }
}

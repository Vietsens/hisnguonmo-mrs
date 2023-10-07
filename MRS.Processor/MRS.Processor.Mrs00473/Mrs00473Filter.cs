using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00473
{
    public class Mrs00473Filter
    {
        public long TIME_FROM { get;  set;  }             // thời gian khóa vp
        public long TIME_TO { get;  set;  }

        public string EXECUTE_LOGINNAME { get;  set;  }

        public List<long> EXECUTE_ROOM_IDs { get; set; } // khoa lấy bc
        public List<long> EXE_ROOM_IDs { get; set; } // khoa lấy bc

        public bool? EXAM_END_TYPE__PAUSE { get; set; } //Khám kết thúc điều trị

        public bool? EXAM_END_TYPE__INTREAT { get; set; } //Khám kết thúc điều trị
    }
}

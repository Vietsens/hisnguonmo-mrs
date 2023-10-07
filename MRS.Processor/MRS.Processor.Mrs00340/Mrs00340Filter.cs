using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00340
{
    
    public class Mrs00340Filter
    {
        public long TIME_FROM { get;  set;  }                 // thời gian thực hiện
        public long TIME_TO { get;  set; }

        public List<long> REQ_ROOM_IDs { get; set; }        // khoa chỉ định
        public List<long> EXE_ROOM_IDs { get; set; }        // khoa thực hiện

        public List<long> REQ_DEPARTMENT_IDs { get; set; }        // khoa chỉ định
        public List<long> EXE_DEPARTMENT_IDs { get; set; }        // khoa thực hiện

        public List<string>  LOGINNAMEs { get;  set;  }       // người thực hiện

        public long? INPUT_DATA_ID_SVT { get; set; } //loại dịch vụ : 1: phẫu thuật; 2: thủ thuật

        public bool? IS_SHOW_FREQUENCE { get; set; }// Hiển thị theo số lần

        public Mrs00340Filter() { }
    }
}

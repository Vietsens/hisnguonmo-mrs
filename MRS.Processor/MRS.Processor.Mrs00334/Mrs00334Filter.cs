using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00334
{
    
    public class Mrs00334Filter
    {
        public long TIME_FROM { get;  set;  }         // thời gian thuwcju hiện
        public long TIME_TO { get;  set;  }

        public List<long> REQUEST_ROOM_IDs { get; set; }  // phòng chỉ định
        public List<long> EXECUTE_ROOM_IDs { get; set; }  // phòng thực hiện
        public List<long> EXE_ROOM_IDs { get; set; }  // phòng thực hiện
        public List<long> PTTT_GROUP_IDs { get;  set;  }    // loại pttt
        public List<string>  LOGINNAMEs { get;  set;  }        // người thực hiện
        public List<long> PATIENT_TYPE_IDs { get;  set;  }  // đối tượng bệnh nhân

        public Mrs00334Filter() { }
    }
}

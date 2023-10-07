using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00416
{
    public class Mrs00416Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public List<long> REQUEST_ROOM_IDs { get;  set;  }  // phòng chỉ định
        public List<long> EXCUTE_ROOM_IDs { get;  set;  }  // phòng thực hiện
        public List<long> PTTT_GROUP_IDs { get;  set;  }    // loại pttt
        public List<string> EXCUTE_USERNAMEs { get;  set;  }        // người thực hiện
        public List<long> PATIENT_TYPE_IDs { get;  set;  }  // đối tượng bệnh nhân

    }
}

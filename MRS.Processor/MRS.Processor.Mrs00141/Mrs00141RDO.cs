using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00141
{
    class Mrs00141RDO
    {
        public string PATIENT_TYPE_NAME { get;  set;  }
        public long PATIENT_TYPE_ID { get;  set;  }
        public int EXAM_IN_COUNT { get;  set;  }
        public int EXAM_OUT_COUNT { get;  set;  }
        public int TREAT_IN_COUNT { get;  set;  }
        public int TREAT_OUT_COUNT { get;  set;  }
        public int SURG_L1_IN_COUNT { get;  set;  }
        public int SURG_L1_OUT_COUNT { get;  set;  }
        public int SURG_L2_IN_COUNT { get;  set;  }
        public int SURG_L2_OUT_COUNT { get;  set;  }
        public int SURG_L3_IN_COUNT { get;  set;  }
        public int SURG_L3_OUT_COUNT { get;  set;  }
        public int SURG_L4_IN_COUNT { get;  set;  }
        public int SURG_L4_OUT_COUNT { get;  set;  }
    }
}

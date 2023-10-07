using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00473
{
    public class Mrs00473RDO
    {
        public string PATIENT_CODE { get;  set;  }
        public string TREATMENT_CODE { get;  set;  }
        public string PATIENT_NAME { get;  set;  }

        public string ROOM_1 { get;  set;  }
        public string ROOM_2 { get;  set;  }
        public string ROOM_3 { get;  set;  }
        public string ROOM_4 { get;  set;  }
        public string ROOM_5 { get;  set;  }

        public string ROOM_6 { get;  set;  }
        public string ROOM_7 { get;  set;  }
        public string ROOM_8 { get;  set;  }
        public string ROOM_9 { get;  set;  }
        public string ROOM_10 { get;  set;  }

        public string ROOM_11 { get;  set;  }
        public string ROOM_12 { get;  set;  }
        public string ROOM_13 { get;  set;  }
        public string ROOM_14 { get;  set;  }
        public string ROOM_15 { get;  set;  }

        public string ROOM_OTHER { get;  set;  }

        public Mrs00473RDO() { }
    }

    public class EXECUTE_ROOM_473
    {
        public long NUMBER { get;  set;  }
        public long ROOM_ID { get;  set;  }
        public string ROOM_NAME { get;  set;  }
        public string ROOM_TAG { get;  set;  }
    }
}

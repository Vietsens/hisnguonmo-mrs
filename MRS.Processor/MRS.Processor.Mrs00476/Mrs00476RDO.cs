using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00476
{
    public class Mrs00476RDO
    {
        public string LOGINNNAME { get;  set;  }
        public string USERNAME { get;  set;  }

        public long ROOM_1 { get;  set;  }
        public long ROOM_2 { get;  set;  }
        public long ROOM_3 { get;  set;  }
        public long ROOM_4 { get;  set;  }
        public long ROOM_5 { get;  set;  }

        public long ROOM_6 { get;  set;  }
        public long ROOM_7 { get;  set;  }
        public long ROOM_8 { get;  set;  }
        public long ROOM_9 { get;  set;  }
        public long ROOM_10 { get;  set;  }

        public long ROOM_11 { get;  set;  }
        public long ROOM_12 { get;  set;  }
        public long ROOM_13 { get;  set;  }
        public long ROOM_14 { get;  set;  }
        public long ROOM_15 { get; set; }
        public Dictionary<string,long> DIC_ROOM { get; set; }

        public decimal TREATMENT_END_EXAM { get;  set;  }    // số hsdt kết thúc
        public decimal SERE_SERV_END_EXAM { get;  set;  }    // số lượt khám kết thúc

        public Mrs00476RDO() {
            DIC_ROOM = new Dictionary<string, long>();
        }
    }

    public class EXECUTE_ROOM_476
    {
        public long NUMBER { get; set; }
        public long ROOM_ID { get; set; }
        public string ROOM_NAME { get; set; }
        public string ROOM_TAG { get; set; }
    }

    public class HIS_SERVICE_REQ_ :HIS_SERVICE_REQ
    {
        public long NUMBER { get; set; }
        public long ROOM_ID { get; set; }
        public string ROOM_NAME { get; set; }
        public string ROOM_TAG { get; set; }
    }
}

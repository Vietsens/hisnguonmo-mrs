using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00349
{
    public class Mrs00349RDO
    {
        public string DATETIME { get;  set;  }

        public string USERNAME { get;  set;  }
        public string LOGINNAME { get;  set;  }

        public string MORNING_START_TIME { get;  set;  }
        public string MORNING_END_TIME { get;  set;  }
        public long? MORNING_AMOUNT_REQUEST {get; set; }

        public string AFTERNOON_START_TIME { get;  set;  }
        public string AFTERNOON_END_TIME { get;  set;  }
        public long? AFTERNOON_AMOUNT_REQUEST { get;  set;  }

        public Mrs00349RDO() { }
    }

    public class MRS00349_SERVICE_REQ
    {
        public string INTRUCTION_DATE { get;  set;  }
        public long INTRUCTION_TIME { get;  set;  }
        public V_HIS_SERVICE_REQ SERVICE_REQ { get;  set;  }
    }
}

using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00156
{
    public class Mrs00156Filter
    {
        public long DATE_FROM { get;  set;  }

        public long DATE_TO { get;  set;  }

        public long EXP_MEST_TYPE_ID { get;  set;  }

        public long EXP_MEST_STT_ID { get;  set;  }

        public List<long> MEDI_STOCK_IDs { get;  set;  }

        public List<long> DEPARTMENT_IDs { get;  set;  }

        public List<long> REQ_ROOM_IDs { get;  set;  }
    }
}

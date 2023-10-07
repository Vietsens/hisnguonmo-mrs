using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00112
{
    public class Mrs00112Filter
    {
        public long? TIME_FROM { get;  set;  }
        public long? TIME_TO { get;  set;  }

        public long? KSK_CONTRACT_ID { get;  set;  }
        public List<long> KSK_CONTRACT_IDs { get;  set;  }

    }
}

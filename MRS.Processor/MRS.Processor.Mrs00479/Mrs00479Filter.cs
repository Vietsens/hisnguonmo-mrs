using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00479
{
    public class Mrs00479Filter
    {
        public long TIME_FROM { get;  set;  }                 // thời gian kết thúc
        public long TIME_TO { get;  set;  }

        public long EXECUTE_DEPARTMENT_ID { get;  set;  }     // khoa thực hiện  

        public Mrs00479Filter() { }
    }
}

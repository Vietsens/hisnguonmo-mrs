using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00615
{
    public class Mrs00615Filter
    {
        public long? TIME_FROM { get;  set;  }
        public long? TIME_TO { get;  set;  }

        public long? REPORT_TYPE_CAT_ID { get; set; }

        public long? TREATMENT_TYPE_ID { get; set; }
        public long? REQUEST_DEPARTMENT_ID { get; set; }
        public long? EXECUTE_DEPARTMENT_ID { get; set; }
        public long? REQUEST_ROOM_ID { get; set; }

        public Mrs00615Filter()
            : base()
        {

        }
    }
}

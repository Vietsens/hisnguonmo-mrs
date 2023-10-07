using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00677
{
    public class Mrs00677Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }

        public long? REQUEST_ROOM_ID { get; set; }

        public long? REQUEST_DEPARTMENT_ID { get; set; }

        public long? EXECUTE_ROOM_ID { get; set; }

        public long? EXECUTE_DEPARTMENT_ID { get; set; }
    }
}

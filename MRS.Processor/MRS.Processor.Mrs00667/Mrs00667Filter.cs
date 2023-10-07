using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00667
{
    public class Mrs00667Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }

        public long? EXECUTE_DEPARRTMENT_ID { get; set; }

        public long? EXECUTE_ROOM_ID { get; set; }

        public Mrs00667Filter() { }
    }
}

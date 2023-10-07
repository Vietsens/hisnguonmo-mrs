using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00661
{
    public class Mrs00661Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public long? END_DEPARTMENT_ID { get; set; }
        public long? EXAM_ROOM_ID { get; set; }
    }
}

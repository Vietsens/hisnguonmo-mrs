using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00729
{
    class Mrs00729Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; }
        public List<long> EXAM_ROOM_IDs { get; set; }
        public long? BRANCH_ID { get; set; }
    }
}

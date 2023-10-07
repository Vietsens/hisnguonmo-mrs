using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00728
{
    class Mrs00728Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }

        public List<string> REQUEST_LOGINNAMEs { get; set; }
        public List<long> MEDICINE_TYPE_IDs { get; set; }
        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }
        public List<long> REQUEST_ROOM_IDs { get; set; }
    }
}

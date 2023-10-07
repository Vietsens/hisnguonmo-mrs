using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00173
{
    public class Mrs00173Filter
    {
        public long? EXECUTE_DEPARTMENT_ID { get; set; } // EXECUTE_DEPARTMENT_ID
        public long? DATE_FROM { get; set; } // FINISH_TIME_FROM
        public long? DATE_TO { get; set; } // FINISH_TIME_TO
        public long? INTRUCTION_TIME_FROM { get; set; } // FINISH_TIME_FROM
        public long? INTRUCTION_TIME_TO { get; set; } // FINISH_TIME_TO
        public long? FINISH_TIME_FROM { get; set; } // FINISH_TIME_FROM
        public long? FINISH_TIME_TO { get; set; } // FINISH_TIME_TO
        public List<long> EXECUTE_ROOM_IDs { get; set; }

        public short? IS_PT_TT { get; set; }//null:all; 1:PT; 0: TT

    }
}

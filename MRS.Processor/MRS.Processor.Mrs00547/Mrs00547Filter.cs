using MOS.MANAGER.HisServiceReq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00547
{
    public class Mrs00547Filter
    {
        public long? REPORT_TYPE_CAT_ID { get; set; }
        public List<long> REPORT_TYPE_CAT_IDs { get; set; }
        public string TYPE_01 { get; set; }
        public string TYPE_02 { get; set; }
        public string TYPE_OTHER { get; set; }
        public bool? IS_TREAT { get; set; }
        public long? EXE_ROOM_ID { get; set; }
        public long? EXECUTE_ROOM_ID { get; set; }

        public string REQUEST_LOGINNAME { get; set; }

        public List<string> REQUEST_LOGINNAMEs { get; set; }

        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }
        public long? REQUEST_DEPARTMENT_ID { get; set; }

        public long? PATIENT_TYPE_ID { get; set; }

        public long? INTRUCTION_TIME_FROM { get; set; }
        public long? INTRUCTION_TIME_TO { get; set; }
        public long? START_TIME_FROM { get; set; }
        public long? START_TIME_TO { get; set; }
        public long? FINISH_TIME_FROM { get; set; }
        public long? FINISH_TIME_TO { get; set; }

    }
}

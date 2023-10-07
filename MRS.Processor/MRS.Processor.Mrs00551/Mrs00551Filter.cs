using MOS.MANAGER.HisServiceReq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00551
{
    public class Mrs00551Filter : HisServiceReqFilterQuery
    {
        public string H_01 { get; set; }
        public string H_02 { get; set; }
        public string H_OTHER { get; set; }
        public string DV { get; set; }
        public long? EXE_ROOM_ID { get; set; }
        public List<string> SERVICE_CODE_MANY_INDEX { get; set; }

        public long? PATIENT_TYPE_ID { get; set; }
    }
}

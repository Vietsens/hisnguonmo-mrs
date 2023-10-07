using MOS.MANAGER.HisServiceReq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00557
{
    public class Mrs00557Filter : HisServiceReqFilterQuery
    {
        public string H_01 { get; set; }
        public string H_02 { get; set; }
        public string H_OTHER { get; set; }
        public string TB { get; set; }
        public string CS { get; set; }
        public string DV { get; set; }
        public long? EXE_ROOM_ID { get; set; }

        public long? PATIENT_TYPE_ID { get; set; }
    }
}

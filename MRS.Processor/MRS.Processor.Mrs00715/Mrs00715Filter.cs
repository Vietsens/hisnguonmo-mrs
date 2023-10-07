using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00715
{
    public class Mrs00715Filter
    {
        public long? EXAM_ROOM_ID { get; set; }
        public long? REQUEST_ROOM_ID { get; set; }
        public List<long> EXAM_ROOM_IDs { get; set; }
        public List<long> REQUEST_ROOM_IDs { get; set; }
        public long? IN_TIME_FROM { get; set; }
        public long? IN_TIME_TO { get;  set;  }
        public long? OUT_TIME_FROM { get;  set;  }
        public long? OUT_TIME_TO { get; set; }
        public long? FEE_LOCK_TIME_FROM { get; set; }
        public long? FEE_LOCK_TIME_TO { get; set; }
        public long? INTRUCTION_TIME_FROM { get; set; }
        public long? INTRUCTION_TIME_TO { get; set; }
        public long? FINISH_TIME_FROM { get; set; }
        public long? FINISH_TIME_TO { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public bool? IS_FINISH { get; set; }
        public bool? IS_MERGE_EXAM_ROOM { get; set; }

        public string DEPARTMENT_CODE__OUTPATIENTs { get; set; }
    }
}

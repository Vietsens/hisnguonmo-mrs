using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00589
{
    public class Mrs00589Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public List<string> ACCIDENT_BODY_PART_CODE__SOFT { get; set; }
        public List<string> ACCIDENT_BODY_PART_CODE__BONE { get; set; }
        public List<string> ACCIDENT_BODY_PART_CODE__BRAIN { get; set; }
        public List<string> ACCIDENT_BODY_PART_CODE__MULTI { get; set; }
        public List<string> DEATH_CAUSE__LATE { get; set; }
        public List<string> DEATH_CAUSE__HEAVY { get; set; }
        public List<string> DEATH_CAUSE__EMERGENCY { get; set; }


        public List<long> DEPARTMENT_IDs { get; set; }
        public List<long> EXAM_ROOM_IDs { get; set; }
        public List<long> EXE_ROOM_IDs { get; set; }


        public long? ACCIDENT_HURT_TYPE_ID { get; set; }

        public List<long> ACCIDENT_HURT_TYPE_IDs { get; set; }


        public List<long> ACCIDENT_RESULT_IDs { get; set; }


        public long? ACCIDENT_RESULT_ID { get; set; }
    }
}

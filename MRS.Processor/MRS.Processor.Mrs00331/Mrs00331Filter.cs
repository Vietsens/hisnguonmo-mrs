using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00331
{
    public class Mrs00331Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }

        public long? EXECUTE_DEPARTMENT_ID { get; set; }

        public long? EXAM_ROOM_ID { get; set; }

        public List<long> EXAM_ROOM_IDs { get; set; }

        public long? BRANCH_ID { get; set; }
        public bool? CHOOSE_TIME { get; set; } //Lọc theo: null: thời gian kết thúc, true: thời gian chỉ định, false: thời gian kết thúc
    }
}

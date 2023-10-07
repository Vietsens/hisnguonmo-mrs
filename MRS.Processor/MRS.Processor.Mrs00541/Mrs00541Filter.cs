using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00541
{
    public class Mrs00541Filter
    {
        public long? BRANCH_ID { get; set; }
        public List<long> BRANCH_IDs { get; set; }
        public long? EXAM_ROOM_ID { get; set; }


        public bool? CHOOSE_TIME { get; set; } //Lọc theo: null: thời gian duyệt giám định, true: thời gian ra viện, false: thời gian duyệt giám định
        public long? FEE_LOCK_TIME_FROM { get; set; }
        public long? FEE_LOCK_TIME_TO { get; set; }
    }
}

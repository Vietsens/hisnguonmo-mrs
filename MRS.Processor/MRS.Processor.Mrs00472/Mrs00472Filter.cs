using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00472
{
    public class Mrs00472Filter
    {
        public long TIME_FROM { get;  set;  }             // thời gian khóa vp
        public long TIME_TO { get; set; }

        public long? DEPARTMENT_ID { get; set; }

        public string SAME_PTTT_GROUP_CODE__1 { get; set; }

        public string SAME_PTTT_GROUP_CODE__2 { get; set; }

        public string SAME_PTTT_GROUP_CODE__3 { get; set; }

        public string SAME_PTTT_GROUP_CODE__4 { get; set; }

        public bool? IS_COUNT_DAY_BORDERAU { get; set; }

    }
}

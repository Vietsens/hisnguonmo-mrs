using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00062
{
    /// <summary>
    /// Sổ vào viện theo khoa
    /// </summary>
    public class Mrs00062Filter
    {
        public long? DEPARTMENT_ID { get; set; }
        public long? FIRST_DEPARTMENT_ID { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }

        public long? TIME_FROM { get;  set;  }
        public long? TIME_TO { get;  set;  }
        public long? IN_TIME_FROM { get; set; }
        public long? IN_TIME_TO { get; set; }

        public long? DATE_OF_WEEK_FROM { get; set; }
        public long? DATE_OF_WEEK_TO { get; set; }
        public long? MY_DEPARTMENT_ID { get; set; }

        public bool? CHECK_EXAM_INTREAT { get; set; }

        public short? CHOOSE_RESULT { get; set; }

        public Mrs00062Filter()
            : base()
        {
        }

        public List<long> EXAM_ROOM_IDs { get; set; }
    }
}

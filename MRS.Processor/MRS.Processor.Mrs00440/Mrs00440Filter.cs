using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00440
{
    public class Mrs00440Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }

        public long? DEPARTMENT_ID { get; set; }

        public bool? IS_BODY_PART_FROM_ICD { get; set; }

        public bool? IS_CAUSE_FROM_ICD { get; set; }

    }
}

using MOS.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00619
{
    public class Mrs00619Filter
    {
        public long? FINISH_TIME_FROM { get; set; }
        public long? FINISH_TIME_TO { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public long? REPORT_TYPE_CAT_ID { get; set; }
        public long? MACHINE_ID { get; set; }
        public List<long> MACHINE_IDs { get; set; }
    }
}

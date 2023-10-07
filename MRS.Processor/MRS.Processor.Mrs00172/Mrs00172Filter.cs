using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00172
{
    public class Mrs00172Filter
    {
        public long? DATE_FROM { get; set; }

        public long? DATE_TO { get; set; }
        public long? SERVICE_GROUP_ID { get; set; }
        public long? TREATMENT_TYPE_ID { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
    }
}

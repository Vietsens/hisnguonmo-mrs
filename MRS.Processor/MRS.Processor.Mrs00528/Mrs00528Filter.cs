using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRS.Processor.Mrs00528
{
    public class Mrs00528Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public long? BRANCH_ID { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public long? TREATMENT_TYPE_ID { get; set; }
    }
}

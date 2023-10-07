using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00194
{
    public class Mrs00194Filter
    {
        public long DATE_TIME_FROM { get; set; }

        public long DATE_TIME_TO { get; set; }

        public long DEPARTMENT_ID { get; set; }

        public List<long> PATIENT_TYPE_IDs { get; set; }
    }
}

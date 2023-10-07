using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00810
{
    public class Mrs00810Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long PATIENT_TYPE_IDs { get; set; }
        public long BRANCH_ID { set; get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00065
{
    public class Mrs00065Filter
    {
        public long? BRANCH_ID { get; set; }

        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }

        public long? OUT_TIME_FROM { get; set; }
        public long? OUT_TIME_TO { get; set; }

        public List<long> BRANCH_IDs { get; set; }
        public bool? IS_T_NGUONKHAC_OPTION__TRUE { get; set; }

        public List<long> PATIENT_TYPE_IDs { get; set; }
    }
}

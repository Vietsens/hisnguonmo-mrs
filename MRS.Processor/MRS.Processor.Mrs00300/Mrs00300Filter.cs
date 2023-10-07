using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00300
{
    public class Mrs00300Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? BRANCH_ID { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; }

        public bool? ADD_DEATH { get; set; }

        public bool? TRUE_FALSE { get; set; }

        public bool? TAKE_PTTT_INFO { get; set; }
    }
}

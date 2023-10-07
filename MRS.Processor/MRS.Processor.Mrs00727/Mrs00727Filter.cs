using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00727
{
    class Mrs00727Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }
        public bool? IS_ELSE_DEBATE { get; set; }
        public bool? IS_MEDICAL_DEBATE { get; set; }
        public bool? IS_BEFORE_SURGERY_DEBATE { get; set; }
        public List<long> DEBATE_TYPE_IDs { get; set; }
        public List<long> BRANCH_IDs { get; set; }
    }
}

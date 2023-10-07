using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00736
{
    class Mrs00736Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public List<long> BRANCH_IDs { get; set; }
        public List<long> OTHER_PAY_SOURCE_IDs { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
    }
}

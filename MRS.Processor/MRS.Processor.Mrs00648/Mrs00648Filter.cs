using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00648
{
    public class Mrs00648Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public bool? IS_NOT_ACTIVE { get; set; }

        public List<long> DEPARTMENT_IDs { get; set; }
    }
}

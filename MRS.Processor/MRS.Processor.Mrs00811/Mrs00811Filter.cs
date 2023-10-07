using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00811
{
    public class Mrs00811Filter
    {
        public long TIME_FROM { set; get; }
        public long TIME_TO { set; get; }
        public bool CHECK_TYPE_TREATMENT { set; get; }
        public List<long> DEPARTMENT_IDs { set; get; }
    }
}

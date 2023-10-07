using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00805
{
    public class Mrs00805Filter
    {
        public long TIME_FROM { set; get; }
        public long TIME_TO { set; get; }
        public List<long> DEPARTMENT_IDs { set; get; }
        public List<long> PATIENT_TYPE_IDs { set; get; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00817
{
    public class Mrs00817Filter
    {
        public long TIME_FROM { set; get; }
        public long TIME_TO { get; set; }
        public long BRANCH_ID { set; get; }
        public List<long> BRANCH_IDs { set; get; }
        public List<long> PATIENT_TYPE_IDs { set; get; }
    }
}

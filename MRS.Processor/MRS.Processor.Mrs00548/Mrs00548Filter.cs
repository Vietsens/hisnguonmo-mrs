using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00548
{
    public class Mrs00548Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public List<string> CASHIER_LOGINNAMEs { get; set; }
    }
}

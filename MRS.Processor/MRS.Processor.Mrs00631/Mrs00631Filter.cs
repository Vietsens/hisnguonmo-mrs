using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00631
{
    public class Mrs00631Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }

        public List<long> PATIENT_TYPE_IDs { get; set; }
        public string CASHIER_LOGINNAME { get; set; }
    }
}

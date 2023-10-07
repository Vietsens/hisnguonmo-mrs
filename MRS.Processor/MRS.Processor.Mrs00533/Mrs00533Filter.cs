using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRS.Processor.Mrs00533
{
    public class Mrs00533Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? BRANCH_ID { get; set; }
        public string CASHIER_LOGINNAME { get; set; }
    }
}

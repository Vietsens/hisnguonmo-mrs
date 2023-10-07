using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRS.Processor.Mrs00526
{
    public class Mrs00526Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? BRANCH_ID { get; set; }
        public string CASHIER_LOGINNAME { get; set; }
        public string LOGINNAME { get; set; }
        public bool? IS_NO_PAY { get; set; }
        public bool? IS_ALL_TREAT { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRS.Processor.Mrs00249
{
    public class COUNT_FEE_LOCK
    {
        public long FEE_LOCK_DATE { get; set; }
        public string END_DEPARTMENT_CODE { get; set; }
        public string FEE_LOCK_LOGINNAME { get; set; }
        public int COUNT_TREATMENT { get; set; }
    }
}

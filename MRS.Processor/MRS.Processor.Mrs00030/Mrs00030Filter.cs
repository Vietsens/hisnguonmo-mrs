using MOS.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00030
{
    public class Mrs00030Filter : FilterBase
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public string CASHIER_LOGINNAME { get; set; }
        public List<string> CASHIER_LOGINNAMEs { get; set; }
        public string LOGINNAME { get; set; }
        public List<string> LOGINNAMEs { get; set; }
        public long BRANCH_ID { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public long? TREATMENT_TYPE_ID { get; set; }
        public bool? HAS_DETAIL { get; set; }
    }
}

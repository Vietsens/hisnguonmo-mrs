using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00737
{
    public class Mrs00737Filter
    {
        public long FEE_LOCK_TIME_FROM { get;  set;  }
        public long FEE_LOCK_TIME_TO { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public List<string> CASHIER_LOGINNAMEs { get; set; }
        public List<long> PAY_FORM_IDs { get; set; }
        public bool? NOT_CALCU_INV { get; set; }
        public long? BRANCH_ID { get; set; }
    }
}

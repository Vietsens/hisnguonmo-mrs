using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00516
{
    public class Mrs00516Filter : FilterBase
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public string CASHIER_LOGINNAME { get; set; }
        public List<string> CASHIER_LOGINNAMEs { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public List<long> PAY_FORM_IDs { get; set; }
        public bool? ADD_SALE_TYPE__KHAC { get; set; }
        public List<long> ACCOUNT_BOOK_IDs { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public List<long> BRANCH_IDs { get; set; }
        public long? BRANCH_ID { get; set; }
    }
}
	
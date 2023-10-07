using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00254
{
    public class Mrs00254Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public List<long> DEPARTMENT_IDs { get; set; }
        public string CASHIER_LOGINNAME { get; set; }
        public long? ACCOUNT_BOOK_ID { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get;  set;  }

        
    }
}

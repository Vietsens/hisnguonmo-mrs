using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00625
{
    public class Mrs00625Filter
    {
        public long TRANSACTION_TIME_FROM { get;  set;  }
        public long TRANSACTION_TIME_TO { get; set; }
        public long? BRANCH_ID { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public long? PACKAGE_ID { get; set; }
        public List<long> EXACT_CASHIER_ROOM_IDs { get; set; }
    }
}

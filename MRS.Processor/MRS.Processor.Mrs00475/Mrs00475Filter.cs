using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00475
{
    public class Mrs00475Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }
        public long? BRANCH_ID { get; set; }
        public long? REQUEST_DEPARTMENT_ID { get; set; }
        /// </summary>
        public List<long> TREATMENT_TYPE_IDs { get; set; }
    }
}

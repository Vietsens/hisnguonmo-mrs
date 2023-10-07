using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00620
{
    public class Mrs00620Filter
    {
        public long MONTH { get; set; }
        public long? BRANCH_ID { get; set; }
        public long? DEPARTMENT_ID { get; set; }
    }
}

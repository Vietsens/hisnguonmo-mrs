using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00309
{
    public class Mrs00309Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? END_DEPARTMENT_ID { get; set; }

        public long? TREATMENT_TYPE_ID { get; set; }
    }
}
	
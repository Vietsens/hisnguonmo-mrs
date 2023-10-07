using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00597
{
    public class Mrs00597Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public bool? IS_TREAT_IN { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }

        public long? DEPARTMENT_ID { get; set; }

        public List<long> DEPARTMENT_IDs { get; set; }
    }
}

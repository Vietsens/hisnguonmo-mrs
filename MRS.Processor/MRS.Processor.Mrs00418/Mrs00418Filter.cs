using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00418
{
    public class Mrs00418Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; }

        public bool TRUE_OR_FALSE { get; set; }

        public List<long> SERVICE_REQ_STT_IDs { get; set; }

        public string EXCLUDE_SERVICE_CODEs { get; set; }

        public bool? IS_SEPRATE_TREAT { get; set; }
    }
}

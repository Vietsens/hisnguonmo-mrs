using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00468
{
    public class Mrs00468Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }

        public long? DEPARTMENT_ID { get; set; }

        public List<long> DEPARTMENT_IDs { get; set; }

        public bool? IS_IN_BED_ROOM { get; set; }

        public string BED_TYPE_CODE__YCs { get; set; }

        public bool? IS_CLINICAL_DEPA { get; set; }
    }
}
